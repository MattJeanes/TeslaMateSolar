# TeslaMateSolar

## NOTE: THIS PROJECT IS HEAVY WIP

TeslaMateSolar is an extension to TeslaMate that allows you to control vehicle charging based on solar data.

It can be configured to work in one or two modes:

Excess: Use excess energy that would otherwise be going out to the grid for charging your Tesla. Calculated using `solar - (load - tesla)`

Priority: This mode still prioritises your house load, but will use any remaining solar energy after that, even if it would feed into a home battery. Calculated using `(grid out - grid in) + tesla)

Note that these two modes are effectively identical without a home battery.
