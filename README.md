# Jib Example for .NET Web Application

This repo is an example of building a containerized ASP.NET Core web application via Jib, a 
tool for building Container images without having a local container runtime installed.

## Prerequisites
* Docker installed and running
* A JRE installed
* Jib installed on your host

## Building the container

Once you have all of these, you can set the `JibPath` property in `jib-example.csproj` to the path to `jib.bat` on Windows, `jib` on non-Windows, then run `dotnet publish -c Release --os alpine --arch x64` to automatically publish the app and run jib on the published assets.

## Running the container

Once you've done the above, `docker run -it -p 80 --rm jib-example` should launch the webapp. you should see logs streaming in your console.

## What happened?

Jib used the `jib.yaml` in this directory to build an image layer by layer, then pushed the layers + manifest to the target specified (which was the local docker host in this case).

If you look at the build output, you see something like this: 

```msbuild
C:\Users\chethusk\source\repos\jib-example\jib-example.csproj(29,5): warning : [WARN] Base image 'mcr.microsoft.com/dotnet/aspnet:6.0.1-alpine3.14-amd64' does not use a specific image digest - build may not be reproducible
  Getting manifest for base image mcr.microsoft.com/dotnet/aspnet:6.0.1-alpine3.14-amd64...
  Building dependencies layer...
  Building configs layer...
  Building static assets layer...
  Building app layer...  
  Using base image with digest: sha256:f9ef02f2f5be50bccec07e0d054648f99921d0245449a45085e3ad36571efe76  
  
  Container entrypoint set to [/app/jib-example]
  Loading to Docker daemon...
```

Jib's warning has been converted into an msbuild warning, but the rest of the log output talks about
* creating specific layers
* setting base images
* setting container entrypoints
* sending the image to the docker daemon

### Layering

You might notice multiple layers in the output. Why is that? Why not just one layer with the publish output?
The primary answer is _speed_. If we create separate layers with specific subsets of data that each change at different 
cadences, then we can push only the layers that change.

The layers I've chosen for this demo
* third-party dependencies
* configuration files
* static assets
* application binaries

This is patterned off of the default Jib integrations in gradle and maven, which use
* dependencies (third-party)
* resources (static/configs)
* .class files (app code)

## Points of interest

* jib.yaml
  * this file tells jib how we want this project to be built. by default it lives in the project root, but it can be anywhere. in a more 'complete' solution we might generate this into the obj directory, for example.
* the `JibPublish` target in the `jib-example.csproj` - 
  * this is a quick and dirty example of how we might create a call to jib, with basic parameter defaults and error reporting.
* on my relatively low-powered laptop, rebuilds of the container take roughly 3 seconds hot, ~7.5 cold.  this is _great_ compared to a multistage docker build already.