This is a port of [TodoMVC in Elm](https://github.com/evancz/elm-todomvc) implemented in F# and targeting Fable and React.
========
The app is live at https://elmish.github.io/sample-react-todomvc.

## Building and running the sample locally 

### one-time changes

1. make sure yarn  is installe and up to date https://yarnpkg.com/en/docs/install
2. npm instaled and up to date

npm install -g npm stable

3. node installed and up to date (example using chocolatey)

choco update nodejs -y

### recurring builds

1. `./build.sh` or `build` or `.\build` (from powershell)  --build defaults to build watch
2. open http://localhost:8080/webpack-dev-server/

`build build` will build only and not run

Hot Module Replacement is active, so have fun watching your saved changes appear immediately.

