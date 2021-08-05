using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Prism.Commands;
using Prism.Mvvm;

namespace WireAntennaUI.ViewModels {
    public class WireLengthUIViewModel : BindableBase {
        private const double speedOfLight = 299792458d;
        private readonly List<HamRadioBand> region1Bands = new List<HamRadioBand>() {
            new HamRadioBand { Band = Band.m2200,  Start = 0.1357, Stop = 0.1378 },
            new HamRadioBand { Band = Band.m630,  Start = 0.472, Stop = 0.479 },
            new HamRadioBand { Band = Band.m160,  Start = 1.810, Stop = 2.000 },
            new HamRadioBand { Band = Band.m80,  Start = 3.500, Stop = 3.800 },
            new HamRadioBand { Band = Band.m60,  Start = 5.3515, Stop = 5.3665 },
            new HamRadioBand { Band = Band.m40,  Start = 7.000, Stop = 7.200 },
            new HamRadioBand { Band = Band.m30,  Start = 10.100, Stop = 10.150 },
            new HamRadioBand { Band = Band.m20,  Start = 14.000, Stop = 14.350 },
            new HamRadioBand { Band = Band.m17,  Start = 18.068, Stop = 18.168 },
            new HamRadioBand { Band = Band.m15,  Start = 21.000, Stop = 21.450 },
            new HamRadioBand { Band = Band.m12,  Start = 24.890, Stop = 24.990 },
            new HamRadioBand { Band = Band.m10,  Start = 28.000, Stop = 29.700 },
            new HamRadioBand { Band = Band.m6,  Start = 50.000, Stop = 51.990 },
            new HamRadioBand { Band = Band.m2,  Start = 144.000, Stop = 146.000 },
            new HamRadioBand { Band = Band.cm70,  Start = 432.000, Stop = 438.000 },
            new HamRadioBand { Band = Band.cm23,  Start = 1240.000, Stop = 1300.000 },
        };

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

        private double minimumTolerance = 0.1d;
        public double MinimumTolerance {
            get => minimumTolerance;
            set => SetProperty(ref minimumTolerance, value, () => Calculate());
        }

        private int selectedPrecision;
        public int SelectedPrecision {
            get => selectedPrecision;
            set => SetProperty(ref selectedPrecision, value, () => Calculate());
        }

        private bool is2200Selected;
        public bool Is2200Selected {
            get => is2200Selected;
            set => SetProperty(ref is2200Selected, value, () => Calculate());
        }
        private bool is630Selected;
        public bool Is630Selected {
            get => is630Selected;
            set => SetProperty(ref is630Selected, value, () => Calculate());
        }
        private bool is160Selected;
        public bool Is160Selected {
            get => is160Selected;
            set => SetProperty(ref is160Selected, value, () => Calculate());
        }
        private bool is80Selected;
        public bool Is80Selected {
            get => is80Selected;
            set => SetProperty(ref is80Selected, value, () => Calculate());
        }
        private bool is60Selected;
        public bool Is60Selected {
            get => is60Selected;
            set => SetProperty(ref is60Selected, value, () => Calculate());
        }
        private bool is40Selected;
        public bool Is40Selected {
            get => is40Selected;
            set => SetProperty(ref is40Selected, value, () => Calculate());
        }
        private bool is30Selected;
        public bool Is30Selected {
            get => is30Selected;
            set => SetProperty(ref is30Selected, value, () => Calculate());
        }
        private bool is20Selected;
        public bool Is20Selected {
            get => is20Selected;
            set => SetProperty(ref is20Selected, value, () => Calculate());
        }
        private bool is17Selected;
        public bool Is17Selected {
            get => is17Selected;
            set => SetProperty(ref is17Selected, value, () => Calculate());
        }
        private bool is15Selected;
        public bool Is15Selected {
            get => is15Selected;
            set => SetProperty(ref is15Selected, value, () => Calculate());
        }
        private bool is12Selected;
        public bool Is12Selected {
            get => is12Selected;
            set => SetProperty(ref is12Selected, value, () => Calculate());
        }
        private bool is10Selected;
        public bool Is10Selected {
            get => is10Selected;
            set => SetProperty(ref is10Selected, value, () => Calculate());
        }
        private bool is6Selected;
        public bool Is6Selected {
            get => is6Selected;
            set => SetProperty(ref is6Selected, value, () => Calculate());
        }
        private bool is2Selected;
        public bool Is2Selected {
            get => is2Selected;
            set => SetProperty(ref is2Selected, value, () => Calculate());
        }
        private bool is70Selected;
        public bool Is70Selected {
            get => is70Selected;
            set => SetProperty(ref is70Selected, value, () => Calculate());
        }
        private bool is23Selected;
        public bool Is23Selected {
            get => is23Selected;
            set => SetProperty(ref is23Selected, value, () => Calculate());
        }

        private ObservableCollection<CutLengthResult> cutLengths;
        public ObservableCollection<CutLengthResult> CutLengths {
            get => cutLengths;
            set => SetProperty(ref cutLengths, value);
        }

        private ObservableCollection<ElectricalLengthResult> electricalLengths;
        public ObservableCollection<ElectricalLengthResult> ElectricalLengths {
            get => electricalLengths;
            set => SetProperty(ref electricalLengths, value);
        }

        private ObservableCollection<HighImpedanceZoneResult> highImpedanceZones;
        public ObservableCollection<HighImpedanceZoneResult> HighImpedanceZones {
            get => highImpedanceZones;
            set => SetProperty(ref highImpedanceZones, value);
        }

        public DelegateCommand calculateCommand;
        public DelegateCommand CalculateCommand => calculateCommand ?? (calculateCommand = new DelegateCommand(Calculate));
        public void Calculate() {
            // Add all selected bands to a list
            var selectedBands = new List<HamRadioBand>();
            if (Is2200Selected)
                selectedBands.Add(region1Bands.First(b => b.Band == Band.m2200));
            if (Is630Selected)
                selectedBands.Add(region1Bands.First(b => b.Band == Band.m630));
            if (Is160Selected)
                selectedBands.Add(region1Bands.First(b => b.Band == Band.m160));
            if (Is80Selected)
                selectedBands.Add(region1Bands.First(b => b.Band == Band.m80));
            if (Is60Selected)
                selectedBands.Add(region1Bands.First(b => b.Band == Band.m60));
            if (Is40Selected)
                selectedBands.Add(region1Bands.First(b => b.Band == Band.m40));
            if (Is30Selected)
                selectedBands.Add(region1Bands.First(b => b.Band == Band.m30));
            if (Is20Selected)
                selectedBands.Add(region1Bands.First(b => b.Band == Band.m20));
            if (Is17Selected)
                selectedBands.Add(region1Bands.First(b => b.Band == Band.m17));
            if (Is15Selected)
                selectedBands.Add(region1Bands.First(b => b.Band == Band.m15));
            if (Is12Selected)
                selectedBands.Add(region1Bands.First(b => b.Band == Band.m12));
            if (Is10Selected)
                selectedBands.Add(region1Bands.First(b => b.Band == Band.m10));
            if (Is6Selected)
                selectedBands.Add(region1Bands.First(b => b.Band == Band.m6));
            if (Is2Selected)
                selectedBands.Add(region1Bands.First(b => b.Band == Band.m2));
            if (Is70Selected)
                selectedBands.Add(region1Bands.First(b => b.Band == Band.cm70));
            if (Is23Selected)
                selectedBands.Add(region1Bands.First(b => b.Band == Band.cm23));

            // Print some help and exit of no band was selected (true when app starts)
            if (selectedBands.Count < 1) {
                ResultText = $"{Environment.NewLine}Check at least one band to perform a calculation";
                CutLengths = new ObservableCollection<CutLengthResult>();
                ElectricalLengths = new ObservableCollection<ElectricalLengthResult>();
                HighImpedanceZones = new ObservableCollection<HighImpedanceZoneResult>();
                return;
            }

            // Get a list with all the corresponding high impedance ranges for the selected bands
            var allHighImpedanceRanges = new List<Range>(selectedBands.Count);
            foreach (var band in selectedBands) {
                var hightImpedanceRange = GetHighImpedanceZones(band.Start, band.Stop, MaxLength, band.Band);
                allHighImpedanceRanges.AddRange(hightImpedanceRange);
            }

            // Calculate the available electrical lengths the antenna may have
            var electricalLengths = GetElectricalLengths(allHighImpedanceRanges, MaxLength);

            // Display the final result of the calculation including the parameters used for the calculation
            ShowResult(electricalLengths, allHighImpedanceRanges, selectedBands);
        }

        /// <summary>
        /// Calculate the high impedance regions of length for the specified frequency band.
        /// High impedance regions are multiples of halvwave length.
        /// </summary>
        /// <param name="minFrequency">Band minimum frequency for calculations</param>
        /// <param name="maxFrequency">Band maximum frequency for calculations</param>
        /// <param name="stopLength">Any high impedance above this wire length is not returned</param>
        /// <returns>A list of pairs of high impedance regions start/stop values</returns>
        public List<Range> GetHighImpedanceZones(double minFrequency, double maxFrequency, double stopLength, Band band) {
            var result = new List<Range>();

            // We should be able to just loop over multiples of the halfwave length and return values as long as neither the start or stop
            // values are smaller then the stopLength.
            // Use wavelength for looping, we need lengths in the end anyway
            var maxWaveLength = speedOfLight / (minFrequency * 1e6);
            var minWaveLength = speedOfLight / (maxFrequency * 1e6);
            var maxHalfWave = maxWaveLength / 2;
            var minHalfWave = minWaveLength / 2;
            var startExclusion = minHalfWave;
            var stopExclusion = maxHalfWave;

            //// If the antenna is shorter then a halfwave, we have no high impedance to worry about
            //if (minHalfWave > stopLength)
            //    return result;

            // The multiplier of the halfwaves
            var i = 1;
            while (startExclusion <= stopLength) {
                result.Add(new Range { Start = startExclusion, Stop = stopExclusion, ContributingBand = band });
                i++;
                startExclusion = minHalfWave * i;
                stopExclusion = maxHalfWave * i;
            };

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
        public List<Range> GetElectricalLengths(IList<Range> highImpedanceRanges, double stopLength) {
            // E.g. for a 2200m antenna, max 100m long, will have no highImpedanceRanges. The available electrical length is unrestricted in such a case.
            if (highImpedanceRanges.Count < 1)
                return new List<Range> { new Range { Start = 0d, Stop = stopLength } };

            // Merging these ranges is so much fun! Lets start by sorting on Start so that the first element is the lowest number.
            // Note the element deep copy since we intend to change the values and we want to leave the original list intact.
            var sortedRanges = highImpedanceRanges.OrderBy(r => r.Start).Select(h => new Range { Start = h.Start, Stop = h.Stop }).ToList();
            // Realize now that if range[i] does not overlap range[i-1], then range[i+1] cannot overlap range[i-1] since the Start
            // value of range[i+1] must be greater to or equal to range[i].
            // We can thus deduce the following algorithm:
            // 1. With the list sorted in increasing order of the Start value, push the first value (smalest Start value) on a stack.
            // 2. For each range do:
            //    a. If the current range does not overlap with the stack top, push it to the stack.
            //    b. If the current range does overlap with the stack top and Stop of the current range is larger then that of the
            //       Stack, update the stack range Stop to that of the current range Stop.
            // 3. When done, the stack contains the merged intervals, sorted in ascending order on Start.
            // The work here should be done when sorting. The rest is linear. I estimate O(n log n) for the merge.
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

        private void ShowResult(IEnumerable<Range> electricalLengths, IEnumerable<Range> highImpedanceRanges, IEnumerable<HamRadioBand> selectedBands) {
            var sb = new StringBuilder();
            sb.AppendLine(" - Region 1 bands taken into account:");
            foreach (var band in selectedBands) {
                sb.AppendLine($"{GetBandName(band.Band),10} ({band.Start,6} - {band.Stop,6} [MHz])");
            }
            sb.AppendLine($" - Max electrical length: {MaxLength} m");
            sb.AppendLine($" - Velocity factor: {VelocityFactor}");
            sb.AppendLine($" - Speed of light: {speedOfLight} m/s");

            var cl = new ObservableCollection<CutLengthResult>();
            foreach (var zone in electricalLengths) {
                var (mean, tol) = GetMeanAndTolerance(zone);
                // Hide results where the tolerance is smaller then what the user wants to see
                if (tol * VelocityFactor >= MinimumTolerance) {
                    //sb.AppendLine($"{length}{tolerance}");
                    cl.Add(new CutLengthResult {
                        OriginalLength = mean * VelocityFactor,
                        Length = string.Format($"{{0:F{SelectedPrecision}}}", mean * VelocityFactor),
                        OriginalTolerance = tol * VelocityFactor,
                        Tolerance = string.Format($"{{0:F{SelectedPrecision}}}", tol * VelocityFactor) }
                    );
                }
            }
            // Assign to data binding for display in grid
            CutLengths = cl;

            var el = new ObservableCollection<ElectricalLengthResult>();
            foreach (var zone in electricalLengths) {
                el.Add(new ElectricalLengthResult {
                    OriginalStart = zone.Start,
                    Start = string.Format($"{{0:F{SelectedPrecision}}}", zone.Start),
                    OriginalStop = zone.Stop,
                    Stop = string.Format($"{{0:F{SelectedPrecision}}}", zone.Stop),
                    OriginalWidth = zone.Width,
                    Width = string.Format($"{{0:F{SelectedPrecision}}}", zone.Width),
                });
            }
            ElectricalLengths = el;

            var hiZ = new ObservableCollection<HighImpedanceZoneResult>();
            foreach (var zone in highImpedanceRanges) {
                hiZ.Add(new HighImpedanceZoneResult {
                    OriginalStart = zone.Start,
                    Start = string.Format($"{{0:F{SelectedPrecision}}}", zone.Start * VelocityFactor),
                    OriginalStop = zone.Stop,
                    Stop = string.Format($"{{0:F{SelectedPrecision}}}", zone.Stop * VelocityFactor),
                    OriginalWidth = zone.Width,
                    Width = string.Format($"{{0:F{SelectedPrecision}}}", zone.Width),
                    OriginalBand = zone.ContributingBand,
                    Band = GetBandName(zone.ContributingBand)
                });
            }
            HighImpedanceZones = hiZ;
            ResultText = sb.ToString();
        }

        public (double, double) GetMeanAndTolerance(Range range) {
            var mean = (range.Start + range.Stop) / 2;
            var tol = Math.Abs(range.Stop - range.Start) / 2;
            return (mean, tol);
        }

        public string GetBandName(Band band) {
            switch (band) {
                case Band.m2200:
                    return "2200 m";
                case Band.m630:
                    return "630 m";
                case Band.m160:
                    return "160 m";
                case Band.m80:
                    return "80 m";
                case Band.m60:
                    return "60 m";
                case Band.m40:
                    return "40 m";
                case Band.m30:
                    return "30 m";
                case Band.m20:
                    return "20 m";
                case Band.m17:
                    return "17 m";
                case Band.m15:
                    return "15 m";
                case Band.m12:
                    return "12 m";
                case Band.m10:
                    return "10 m";
                case Band.m6:
                    return "6 m";
                case Band.m2:
                    return "2 m";
                case Band.cm70:
                    return "70 cm";
                case Band.cm23:
                    return "23 cm";
                default:
                    return "Unknown band";
            }
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
        private Band contributingBand;
        public Band ContributingBand {
            get => contributingBand;
            set => SetProperty(ref contributingBand, value);
        }
    }

    public enum Precision {
        Meter = 0,
        Decimeter = 1,
        Centimeter = 2,
        Milimeter = 3,
    }

    public class HamRadioBand : BindableBase {
        private Band band;
        public Band Band {
            get => band;
            set => SetProperty(ref band, value);
        }

        private double start;
        public double Start {
            get => start;
            set => SetProperty(ref start, value);
        }
        private double stop;
        public double Stop {
            get => stop;
            set => SetProperty(ref stop, value);
        }
    }

    public enum Band {
        m2200,
        m630,
        m160,
        m80,
        m60,
        m40,
        m30,
        m20,
        m17,
        m15,
        m12,
        m10,
        m6,
        m2,
        cm70,
        cm23,
    }

    public class CutLengthResult {
        public double OriginalLength { get; set; }
        public string Length { get; set; }
        public double OriginalTolerance { get; set; }
        public string Tolerance { get; set; }
    }

    public class ElectricalLengthResult {
        public double OriginalStart { get; set; }
        public string Start { get; set; }
        public double OriginalStop { get; set; }
        public string Stop { get; set; }
        public double OriginalWidth { get; set; }
        public string Width { get; set; }
    }

    public class HighImpedanceZoneResult : ElectricalLengthResult {
        public string Band { get; set; }
        public Band OriginalBand { get; set; }
    }

#if DEBUG
    public class WireLengthUIViewModelDesign : WireLengthUIViewModel { }
#endif
}
