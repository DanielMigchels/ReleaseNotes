# Release Notes
      
Release notes management application built with Angular and .NET 9. Manage changes in projects and export structured release notes in PDF.

<img style="width: 600px;" src="ReleaseNotes.Docs/demo.gif">

## How to Run

Instructions on how to run the application.

### Docker Compose
Compiles source code, builds docker image, and runs it along with its dependencies on your docker instance.
```bash
docker-compose up
```

App becomes available on port 8080 and should be reachable through HTTP. (http://localhost:8080)

### Helm Chart
Installs the app on your Kubernetes cluster.
```bash
helm install releasenotes .\ReleaseNotes.Helm\ --namespace releasenotes --create-namespace
```
App becomes available on port 32524 and should be reachable through HTTP. (http://localhost:32524)

## Future Enhancements
- Automatically track changes through synchronisation with Git repository.