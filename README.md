## Usage

[Helm](https://helm.sh) must be installed to use the charts.  Please refer to
Helm's [documentation](https://helm.sh/docs) to get started.

Once Helm has been set up correctly, add the repo as follows:

    helm repo add <alias> https://mdekrey.github.io/helm-charts

For ease of use, `<alias>` will be listed as `mdekrey` for the rest of this readme:

    helm repo add mdekrey https://mdekrey.github.io/helm-charts

If you had already added this repo earlier, run `helm repo update` to retrieve
the latest versions of the packages.  You can then run `helm search repo
mdekrey` to see the charts.

To install the <chart-name> chart:

    helm install my-<chart-name> mdekrey/<chart-name>

To uninstall the chart:

    helm delete my-<chart-name>


### Single-Container

A chart that runs a single container and exposes it as a service and ingress. Supports TLS. Also supports HorizontalPodAutoscaler, as the default helm template.

To install this chart (Powershell Core):

    $fullImageName = 'alexwhen/docker-2048'
    $imageTag = 'latest'
    $k8sNamespace = 'my-site-ns'
    $chartName = 'my-site'
    $sslClusterIssuer = 'letsencrypt'
    $domain = '2048.example.com'

    helm install -n $k8sNamespace $chartName --create-namespace mdekrey/single-container `
        --set-string "image.repository=$($fullImageName)" `
        --set-string "image.tag=$imageTag" `
        --set-string "ingress.annotations.cert-manager\.io/cluster-issuer=$sslClusterIssuer" `
        --set-string "ingress.hosts[0].host=$domain"

### Namecheap Cert-Manager hooks

Originally from [kelvie/cert-manager-webhook-namecheap](https://github.com/kelvie/cert-manager-webhook-namecheap), from [cert-manager-webhook-namecheap](https://github.com/jamesgoodhouse/cert-manager-webhook-namecheap).

Deploy the webhook:

    helm install -n cert-manager namecheap-webhook --repo https://mdekrey.github.io/helm-charts cert-manager-webhook-namecheap

Deploy the issuer:

    helm install --set email=yourname@example.com -n cert-manager letsencrypt-namecheap-issuer deploy/letsencrypt-namecheap-issuer/

In NameCheap, go to Profile -> Tools and set up API access. (Note that you'll need to whitelist the public IP of the k8s cluster to use the webhook), and set the secret:

    apiVersion: v1
    kind: Secret
    metadata:
      name: namecheap-credentials
      namespace: cert-manager
    type: Opaque
    stringData:
      apiKey: my_api_key_from_namecheap
      apiUser: my_username_from_namecheap

Create a staging certificate for testing:

    apiVersion: cert-manager.io/v1
    kind: Certificate
    metadata:
      name: wildcard-cert-stage
      namespace: default
    spec:
      secretName: wildcard-cert-stage
      commonName: "*.<domain>"
      issuerRef:
        kind: ClusterIssuer
        name: letsencrypt-stage
      dnsNames:
      - "*.<domain>"
