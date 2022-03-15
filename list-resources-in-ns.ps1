Param (
    [String]
    $namespace
)

$apiResources = $(kubectl api-resources --verbs=list --namespaced -o name)

foreach ($resource in $apiResources) {
    kubectl get --show-kind --ignore-not-found -n $namespace $resource -o name
}
