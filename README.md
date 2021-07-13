# WireAntennaCalculator 

This program calculates the best length for a so-called random wire antenna. 

Download the latest release on https://sa5nnn.github.io/WireAntennaCalculator/

   The download uses Windows 10 MSIX packaging technology to keep your app up-to-date. Each time you start the app it checks for updates. If any are available, they will be installed after you restart the application.


# Background 

So, there is that nice antenna type called random length wire antenna. Obviously, such an antenna would consist of at least one single antenna wire, cut to a random length. 

This antenna type has a sibling often called a long wire antenna. A long wire antenna requires a length greater than a quarter-wavelength (λ/4) or half-wavelength (λ/2) of the radio waves (most consider a true long wire to be least one wavelength), whereas random wire antennas have no such constraint. 

As we start looking at electromagnetic theory it soon becomes clear that this type of random wire antenna has a few issues that must be worked on, or they cannot function very well. Especially as a transmission antenna. 


# The dipole antenna

First, consider the dipole antenna, and we will invest quite a bit of time in understanding it because it is what our random length wire antenna is not, and that matters a lot for understanding random wires.

The dipole antenna is a center feed half-wavelength balanced antenna. This means that the antenna is specifically designed for one wavelength e.g., 20m. The antenna is split in the middle where it is fed from the transmitter, one pole connected to each of the two halves.

When a dipole is transmitting it will build up a voltage at the ends while current will be at its maximum at the center of the feed point. This is mostly because the antenna is resonant and has a standing wave of half the design wavelength. This also makes the antenna efficient as a radiator.

The characteristic impedance of the dipole is typically around 73 Ohm but in practice will vary between 45 and 100 Ohms due to the placement. Normal transmission lines have 50 Ohms characteristic impedance and often can be used without an impedance transformer since the reflection is so small (typically below 2:1 VSWR). Though if using an unbalanced feedline such as a coax cable, the coax shield will be directly connected to one of the halves of the dipole and therefore radiate all the way down to the transmitter, creating hazardous EM field strengths for the operator or cause RF burns that can be both painful and take a long time to heal. A RF choke or a (1:1) balun can be used to stop the RF from using the coax shield. With the advent of digital modes, the operator will often use e.g., a laptop that is connected to the transmitter. If RF comes down the coax shield it will severely disturb the computer and may even cause damage to it. Any length of coax cable after the choke or balun should be placed perpendicular to the dipole's wires otherwise, they will pick up RF from the EM field and once again cause problems for the operator. Grounding the coax shield or a second choke at the transmitter could be used to also stop this if necessary.

The dipole antenna also has a bit of gain 2.15 dBi to be precis. The most gain is directed perpendicular from the antenna, and along the wire there is a null with no gain at all. This is useful for eliminating a strong EM source, but also means that the antenna needs to be relocated if other directions for communication are wanted. This is assuming a horizontal orientation of the antenna. It is also possible to use the dipole vertically. In such a case the radiation pattern is ideal for DX: nothing up or down, most gain towards the horizon equally in all directions. The only waste being, half of the gain directed down into the ground, will not be beneficial. It might be worth noting that the term gain used here, does not equate to amplification of the signal, merely the EM field is redirected so that more radiates in certain directions and less in others. In total the dipole can not amplify the signal since it is a passive device.

The dipole need no ground system to function as both halves of the antenna wire are being used. A ground plane antenna would have one half as a wire but rely on the ground for the rest.

One downside is that the antenna needs to be hung around half-wavelength (λ/2) from the ground or the interaction with the ground will begin to short-circuit the emitted field towards ground reducing its effectiveness as well as effecting the radiation pattern and characteristic impedance.

It is true for all emitting wires; they need to be clear from the ground or they will be less efficient in generating an EM field.

Another downside is that it is mono-band, and you cannot use a 20m dipole on 40m or any other band. It needs to be replaced with a different one specific to the new band.


# The feed point moved

Consider the dipole above in a horizontal configuration. What happens if we move the feed point from the dead center towards one of the ends? Well, as it turns out, the characteristic impedance increases. And it does so all the way until you need the antenna from the end. At the end, the impedance varies greatly due to the configuration of the antenna, and one would expect somewhere between 1k to 5k Ohm characteristic impedance at the feed point.

This end fed variant of the dipole comes in many variants among them our long wire and random wire antennas. 

# The end fed half wave 

An attempt to match the variable impedance at some specific set value and use the antenna for the band where it is a halfwave dipole would result in a known as an end fed halfwave antenna or EFHW. There are also varieties for harmonics of the fundamental design frequency. Harmonics are always higher frequencies then the fundamental and therefore the antenna will house multiples of the fundamental wavelength. This creates very distinct lobs in the radiation pattern with nulls in between. This can be very determinantal is the stations you are looking for are in the nulls, or the antenna is physically changing orientation e.g., due to wind, such that the station goes out and in of the nulls. With more than a handful of wavelength fitting in the wire the lobs are exceptionally large and narrow, and the arrangement becomes less useful in practical terms. 

# The random length 

The random length is not random at all, the entire name is totally misleading! Sorry, but I did not come up with it. The designer probably just ignored the length all together and discovered that if an antenna tuner (actually a matching network) were used, mostly the tuner would do its thing and of you went! This was an awesome discovery since now the antenna was a multi-bander. You could get out and hang it up, then tune pretty much anything you wanted and mostly it worked. Mostly... There were a few snags, some bands just would not tune no matter what. Changing the length of the wire would make these untunable bands work, possibly making other, previously tunable bands, stop working. In some instances, a working antenna would tune on one tuner, but not another. 

The issue they encountered is something that you can figure out on your own using the above information. The issue was the impedance at the end as at certain frequencies so high that the matching network in the tuner where unable to match it. Different tuners also have different ranges of impedance they can match. The solution to this problem is to make sure we avoid lengths of the wire that is multiples of halfwaves of the frequencies we want to transmit on. That means all those 1k to 5k Ohm impedances we need to avoid. 

## Not-quite-so-random length wire antenna 

So, let us start off with a random length wire designed to fit the 20m band. In region 1 the 20m band consists of frequencies between 14.000 MHz and 14.350 MHz. Depending on if you use FT8 or telephone maybe not the entire range of frequencies is important, but for completeness let us assume we want any frequency in the band. 

What we wanted to avoid was multiples of halfwave lengths. 10m is approximately the halfwave length and multiples of it would be 10, 20, 30, 40, 50, 60 and so on. But due to the band we actually have to consider all wavelengths between 20.89146m and 21.41375m. It may be ok to approximate this band with just using 21m as a medium wavelength, but to make sure do the math on both end frequencies and mark anything in between as “forbidden” lengths. 

So, to begin, start with 20.89146 and divide by 2 to get 10.44573m and mark all multiples of this length as forbidden. Then add the other lengths 21.41375 divided by 2 to get 10.706875m. No wire length between 10.44 and 10.71m or any multiple thereof such as 20.88 to 21.42m and so on. 

This becomes a bit tedious to calculate, and if you want to use multiple bands you must do the same calculations for those bands and come up with their forbidden ranges. Then the correct length would be an “random” length that did not end up being in any of the intervals for any of the bands you want to use. 

In the end, it must also be practical to cut and use the wire. A length that is only a few centimeters away from adjacent forbidden regions can highly likely get cut to the wrong length or be hung such that the effective length changes slightly to inside a forbidden region. 

The most effective length is those that are in between forbidden regions with the greatest possible margin. Those can be cut a little bit too long or short, hung so that the effective length is not optimal, while still allowing sufficient impedance margin for the matching network in the tuner to do its thing. 

In the end, there are no random lengths that are fitting, but a set of golden lengths that provide the largest margin for errors. The end user should pick one of these and have fun! 

NOTE that in the calculations we use theoretical values such that we arrive at electrical lengths. This may be confusing since it will vary from what should really be cut. The difference is the speed of light in the material, the velocity factor. For a PVC insulated copper wire, we usually use 0.95 as the velocity factor. Whatever electrical length you want, select it from the calculations and then multiply with the velocity factor to get the correct length to cut for that specific wire. In other words, the real wire will be slightly shorter than the electrical length. 

This program will aid you in selecting bands for your antenna and calculate the forbidden regions and present them in an easy to view form as well as provide you with the golden cut lengths for your design bands. 

 

# Random length wire antenna disadvantages 

In the real world, not everybody uses random length wire antennas because they have some disadvantages that other designs do not have. 

## The tuner (matching network) 

 One problem is the need for a tuner (matching network). Any tuner will incur a loss, ranging from a few percent to maybe as high as 50%. The more inductors the matching network needs, the worse the losses since each turn around a ferrite code will incur a loss. 

Generally speaking, shorter antennas require more inductance, so matching 80m on a 10m wire will incur a large loss of power in the tuner. If you have ample power this might not be an issue, unless the tuner itself has problems dissipating the power as it turns into heat. In a QRP setup, your 5 or 10 W power budget will go down the drain and you will not be heard. With a decent wire length, a QRP setup should not be a problem since you will be nowhere near 50% losses, more like a few percent. 

The tuner also has a weight and may need to be elevated if the feed point is high up. This puts physical strain on the wire and may cause damage in windy conditions. For portable use, the tuner may also be too bulky and heavy for transporting around in the terrain. 

## Radiation pattern 

Another issue may be the radiation pattern. As soon as we use a band where half a wavelength fits more than one time, the EM field will divide into lobs. First two, then three and so forth. The more times they fit in the wire length, the more fingers of radiation. Also, there will be nulls where there is no radiation at all between these “fingers.” If you use the antenna and it works, then this is not an issue. But if you think the antenna will always have a nice radiation pattern like that of a dipole, then you are in for a surprise. In general, the nature of this antenna and how it is configured will create a complex radiation pattern. Especially considering grounding and/or so-called counterpoises. 

On the up side, the complexity leaves a lot of room for the operator to experiment and learn a great deal of how the antenna system works (or not). 

 

# Conclusion 

The random length wire antenna is a so-called compromise antenna that promises multi-band access from a single piece of wire with one end hooked up to the radio. Very lightweight, easy to setup and versatile. 

In reality it does so reasonably well only for a few bands while not being particularly efficient at others. 

For the QRP aficionados there are more efficient designs, but they are mono-band and need more work to set up. If such an antenna ends up in the closet and never gets used, then what does efficiency matter? It could just as well have been made entirely out of porcelain. 

Just as a repair man needs a chest of tools, a ham radio operator needs a set of antennas. Depending on the mode of operator and the conditions of the location, the “right” antenna can be selected. The random length wire antenna is necessary in such a tool chest. How much it gets used depends on the circumstances. When used correctly it can compete with a dipole to the point where one could not tell on apart from the other. 

# Finale! 

By the power invested in me as a certified ham radio operator I hear by officially rename the random length wire antenna: The golden length wire antenna!!! 
