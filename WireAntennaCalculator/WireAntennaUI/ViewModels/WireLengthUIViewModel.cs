using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prism.Commands;
using Prism.Mvvm;

namespace WireAntennaUI.ViewModels {
    public class WireLengthUIViewModel : BindableBase {
        private const double speedOfLight = 299792458d;

        public WireLengthUIViewModel() {
            // Setting Centimeter precision also triggers calculation
            SelectedPrecision = 2;
        }

        private string resultText = "Press the Calculate button to perform random wire antenna length calculation";
        public string ResultText {
            get => resultText;
            set => SetProperty(ref resultText, value);
        }

        private double maxLength = 100d;
        public double MaxLength {
            get => maxLength;
            set => SetProperty(ref maxLength, value, () => Calculate());
        }

        private double velocityFactor = 0.95d;
        public double VelocityFactor {
            get => velocityFactor;
            set => SetProperty(ref velocityFactor, value, () => Calculate());
        }

        private int selectedPrecision;
        public int SelectedPrecision {
            get => selectedPrecision;
            set => SetProperty(ref selectedPrecision, value, () => Calculate());
        }

        private double twentyMeterLowFrequency = 14.000;
        public double TwentyMeterLowFrequency {
            get => twentyMeterLowFrequency;
            set => SetProperty(ref twentyMeterLowFrequency, value);
        }
        private double twentyMeterHighFrequency = 14.350;
        public double TwentyMeterHighFrequency {
            get => twentyMeterHighFrequency;
            set => SetProperty(ref twentyMeterHighFrequency, value);
        }

        private double fortyMeterLowFrequency = 7.000;
        public double FortyMeterLowFrequency {
            get => fortyMeterLowFrequency;
            set => SetProperty(ref fortyMeterLowFrequency, value);
        }
        private double fortyMeterHighFrequency = 7.200;
        public double FortyMeterHighFrequency {
            get => fortyMeterHighFrequency;
            set => SetProperty(ref fortyMeterHighFrequency, value);
        }

        private double eightyMeterLowFrequency = 3.500;
        public double EightyMeterLowFrequency {
            get => eightyMeterLowFrequency;
            set => SetProperty(ref eightyMeterLowFrequency, value);
        }
        private double eightyMeterHighFrequency = 3.800;
        public double EightyMeterHighFrequency {
            get => eightyMeterHighFrequency;
            set => SetProperty(ref eightyMeterHighFrequency, value);
        }

        public DelegateCommand calculateCommand;
        public DelegateCommand CalculateCommand => calculateCommand ?? (calculateCommand = new DelegateCommand(Calculate));
        public void Calculate() {
            var twentyHighImpedance = GetHighImpedanceZones(TwentyMeterLowFrequency, TwentyMeterHighFrequency, MaxLength);
            var fortyHighImpedance = GetHighImpedanceZones(FortyMeterLowFrequency, FortyMeterHighFrequency, MaxLength);
            var eightyHighImpedance = GetHighImpedanceZones(EightyMeterLowFrequency, EightyMeterHighFrequency, MaxLength);
            var allHighImpedance = twentyHighImpedance.Concat(fortyHighImpedance.Concat(eightyHighImpedance));
            var electricalLengths = GetElectricalLengths(allHighImpedance, MaxLength);
            ShowResult(electricalLengths, allHighImpedance);
        }


        /// <summary>
        /// Calculate the high impedance regions of length for the specified frequency band.
        /// High impedance regions are multiples of halvwave length.
        /// </summary>
        /// <param name="minFrequency">Band minimum frequency for calculations</param>
        /// <param name="maxFrequency">Band maximum frequency for calculations</param>
        /// <param name="stopLength">Any high impedance above this wire length is not returned</param>
        /// <returns>A list of pairs of high impedance regions start/stop values</returns>
        public List<Range> GetHighImpedanceZones(double minFrequency, double maxFrequency, double stopLength) {
            var result = new List<Range>();

            // We should be able to just loop over multiples of the halfwave length and return values as long as neither the start or stop
            // values are smaller then the stopLength.
            // Use wavelength for looping, we need lengths in the end anyway
            var maxWaveLength = speedOfLight / (minFrequency * 1e6);
            var minWaveLength = speedOfLight / (maxFrequency * 1e6);
            // The multiplier of the halfwave length
            var i = 1;
            while (i < 100) {
                var maxHalfWave = maxWaveLength / 2;
                var minHalfWave = minWaveLength / 2;
                var startExclusion = minHalfWave * i;
                var stopExclusion = maxHalfWave * i;
                // Exit the loop if the high impedance zones are beyond the target wire length
                if (startExclusion > stopLength) break;
                result.Add(new Range { Start = startExclusion, Stop = stopExclusion });
                i++;
            }
            return result;
        }

        /// <summary>
        /// Calculate the possible cut lengths for the wire
        /// </summary>
        /// <param name="highImpedanceRanges">A list of ranges we must not cut the length to</param>
        /// <param name="stopLength">The maximum wire length to concider</param>
        /// <returns>A list of intervals where the antenna could be cut to without it being in any of the high impedance zones</returns>
        /// <remarks>
        /// Lets assume the ranges in the highImpedanceRanges may overlap entirely or partially, all or some. This is realistic since we may end up
        /// calculating the high impedance ranges for multiple bands where multple bands may share the same exclusion zone, at least partially.
        /// </remarks>
        public List<Range> GetElectricalLengths(IEnumerable<Range> highImpedanceRanges, double stopLength) {
            // Merging these ranges is so much fun! Lets start by sorting on Start so that the first element is the lowest number.
            var sortedRanges = highImpedanceRanges.OrderBy(r => r.Start).ToList();
            // Realize now that if range[i] does not overlap range[i-1], then range[i+1] cannot overlap range[i-1] since the Start
            // value of range[i+1] must be greater to or equal to range[i].
            // We can thus deduce the following algorithm:
            // 1. With the list sorted in increasing order of the Start value, push the first value (smalest Start value) on a stack.
            // 2. For each range do:
            //    a. If the current range does not overlap with the stack top, push it to the stack.
            //    b. If the current range does overlap with the stack top and Stop of the current range is larger then that of the
            //       Stack, update the stack range Stop to that of the current range Stop.
            // 3. When done, the stack contains the merged intervals, sorted in ascending order on Start.
            var stack = new Stack<Range>(sortedRanges.Count);
            stack.Push(sortedRanges[0]);
            foreach (var range in sortedRanges) {
                var top = stack.Peek();
                if (top.Stop < range.Start) {
                    // No overlap, next range starts after this one stops
                    stack.Push(range);
                } else if (top.Stop < range.Stop) {
                    // Overlap, update to range Stop if larger, otherwise just skip to next
                    top.Stop = range.Stop;
                    stack.Pop();
                    stack.Push(top);
                }
            }
            var mergedHighImpedanceZones = new List<Range>(stack).OrderBy(r => r.Start);

            // Ok, so now we have a list of intervals that are high impedance, we need the reverse, all the intervals that are the compilment
            // for the list of ranges from 0 to stopLength.
            var result = new List<Range>();
            var lastStart = 0d;
            foreach (var highImpedance in mergedHighImpedanceZones) {
                // Create a range from the last start to where the next high impedance range starts.
                result.Add(new Range { Start = lastStart, Stop = highImpedance.Start });
                // Skip the high impedance range and next time start a new range at the end of the high impedance range
                lastStart = highImpedance.Stop;
            }
            // Do not add ranges above the max design length, they will be wrong anyway
            if (lastStart < MaxLength) {
                result.Add(new Range { Start = lastStart, Stop = stopLength });
            }
            return result;
        }

        private void ShowResult(IEnumerable<Range> electricalLengths, IEnumerable<Range> highImpedanceRanges) {
            var sb = new StringBuilder();
            sb.AppendLine("Random length wire antenna design parameters:");
            sb.AppendLine(" - Region 1 bands taken into account: 80 m, 40 m, 20 m");
            sb.AppendLine($" - Max electrical length: {MaxLength} m");
            sb.AppendLine($" - Velocity factor: {VelocityFactor}");
            sb.AppendLine($" - Speed of light: {speedOfLight} m/s");

            sb.AppendLine().AppendLine("Golden cut lengths:");
            sb.AppendLine($"{"Cut length [m]",14}{"Tolerance +/- [m]",18}");
            foreach (var zone in electricalLengths) {
                var meanAndTol = GetMeanAndTolerance(zone);
                var length = string.Format($"{{0,14:F{SelectedPrecision}}}", meanAndTol.Item1 * VelocityFactor);
                var tolerance = string.Format($"{{0,18:F{SelectedPrecision}}}", meanAndTol.Item2 * VelocityFactor);
                sb.AppendLine($"{length}{tolerance}");
            }

            sb.AppendLine().AppendLine("Available electrical lengths:");
            sb.AppendLine($"{"Start [m]",9}{"Stop [m]",9}{"Span [m]",9}");
            foreach (var zone in electricalLengths) {
                var start = string.Format($"{{0,9:F{SelectedPrecision}}}", zone.Start);
                var stop = string.Format($"{{0,9:F{SelectedPrecision}}}", zone.Stop);
                var width = string.Format($"{{0,9:F{SelectedPrecision}}}", zone.Width);
                sb.AppendLine($"{start}{stop}{width}");
            }

            sb.AppendLine().AppendLine("Do not cut the wire at any length within these zones, or you may not be able to tune the wire due to excessive impedance (> 4 kOhm).");
            sb.AppendLine("High impedance zones:");
            sb.AppendLine($"{"Start [m]",9}{"Stop [m]",9}{"Span [m]",9}");
            foreach (var zone in highImpedanceRanges) {
                var start = string.Format($"{{0,9:F{SelectedPrecision}}}", zone.Start);
                var stop = string.Format($"{{0,9:F{SelectedPrecision}}}", zone.Stop);
                var width = string.Format($"{{0,9:F{SelectedPrecision}}}", zone.Width);
                sb.AppendLine($"{start}{stop}{width}");
            }
            ResultText = sb.ToString();
        }

        public Tuple<double, double> GetMeanAndTolerance(Range range) {
            var mean = (range.Start + range.Stop) / 2;
            var tol = Math.Abs(range.Stop - range.Start) / 2;
            return new Tuple<double, double>(mean, tol);
        }
    }

    public class Range : BindableBase {
        private double start;
        public double Start {
            get => start;
            set {
                if (SetProperty(ref start, value)) {
                    Width = Math.Abs(Stop - Start);
                }
            }
        }
        private double stop;
        public double Stop {
            get => stop;
            set {
                if (SetProperty(ref stop, value)) {
                    Width = Math.Abs(Stop - Start);
                }
            }
        }
        private double width;
        public double Width {
            get => width;
            set => SetProperty(ref width, value);
        }
    }

    public enum Precision {
        Meter = 0,
        Decimeter = 1,
        Centimeter = 2,
        Milimeter = 3,
    }

#if DEBUG
    public class WireLengthUIViewModelDesign : WireLengthUIViewModel {
    }
#endif
}
