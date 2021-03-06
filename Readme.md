# Versatile HTTP Web Server for Linux

A port of my web server to linux

A simple HTTP web server made using C#

Now with settings

If you want to change the port which the server should listen to change port value in settings.ini

You can execute commands remotely.

## Contributions

Thanks to @Harry260 for making the custom error pages

## Dependencies

For Standard release :

*.net runtime

*miniupnpc for using port forward

For Self contained release :

*miniupnpc for using port forward

For instaling miniupnpc :

in ubuntu and other distros with apt package manager, Run

```shell
sudo apt install miniupnpc
```

In other distros

coming soon(or later)

## Releases

There are two type of Releases:

*Self contained release

*Standard release

Self contained releases are the releases which can run without .net runtime. It requires more space

Standard releases are the releases which want .net runtime to run. It requires less space

## Start the server

To run the server. run

```shell
./Start
```

in the root of the extracted folder.

## Build instructions

dotnet sdk is important to build the application. To install it run 

```shell
./Build -I
```
in the project root using terminal. It will install dotnet sdk 5.0 and dotnet runtime 3.1


To build this project as a self contained build. run  

```shell
./Build --cb VHWSL v1.0.0
```

in the project root using terminal


To build this project. run

```shell
./Build --ncb VHWSL v1.0.0
```

in the project root using terminal

## Changelog (v2.0.0)

*Added function to port forward . To enable port forwarding change the value of Port forward to true

*Fixed the logging error

Made with tutorial of <https://youtu.be/HFnJLv2Q1go>
