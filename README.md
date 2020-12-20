# Drone UDP Client Unity3D
Currently works with Tell Edu SDK 2.0

* Uses native .Net libraries 
Tested on Windows x64 and Android Oreo + devices (ARM64 and IL2CPP , using .Net4.x build)

Commands are just string commands converted to bytes[]

## Inportant Notes
Unity crashes if it's unable to connect to drone

## To Do : 

* Update code to support multithreading
* Add check in case connection fails, currenlty unity crashes on failed connection as udp client runs on main thread