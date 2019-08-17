function Help {
    echo "Switches and options: "
    echo "    -clean                              Clean the solution"
    echo "    -configuration [Debug|Release]      Use Debug or Release mode when building"
    echo "    -restore                            Restore all dependencies"
    echo "    -skiptests                          Skip execution of tests - faster"
    echo "    -help                               Print this text"
    echo "    -arch [Any CPU|x86|x64|ARM|ARM64]   Select the architecture to build for"
    echo "\n"
    echo "All other arguments are passed through to MSBuild"
}