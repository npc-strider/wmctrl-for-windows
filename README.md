# wmctrl-for-windows
This is a fork of https://github.com/ebranlard/wmctrl-for-windows with two new flags added for PID and process title :D

Command line implementation of wmctrl for windows (pseudo-equivalent).
Only few features of the linux tool wmctrl are implemented so far, that is:
    - List the window titles, process name and ID
	- Switch the focus to a given window (Based on process title, process name or PID)

The program is written in C-sharp.
This repository contains the latest binary and the source code (compatible Mono or Microsoft).

## Binaries
You can get the binaries from the releases tab of this repo

## Features/ usage
        
usage: wmctrl [options] [args]

options:
  -h          : show this help
  -l          : list windows
  -a <PNAME>  : switch to the window of the process name <PNAME>
  -b <PTITLE> : switch to the window of the process title <PTITLE>
  -p <PID>    : switch to the window of the process ID <PID>



## Compilation
Compile (using Mono or Miscrosoft Visual Studio tools).
You can use the Makefile provided in this repository. 
(You need csc.exe or mcs.exe in your system path)

## Installation
Put the exe file somewhere in your system path.
