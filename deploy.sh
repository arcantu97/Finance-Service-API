#!/usr/bin/env bash
set -euo pipefail

# ====== CONFIGURA ESTAS VARIABLES ======
RG="FinanceRG"
ACR="financeregistry123"                     # sin .azurecr.io
APP="finance-api"
IMAGE_NAME="financeservice"
TAG="latest"
LOCATION="eastus"
# =======================================

ACR_LOGIN_SERVER="$ACR.azurecr.io"
FULL_IMAGE="$ACR_LOGIN_SERVER/$IMAGE_NAME:$TAG"

echo "👉 Build (linux/amd64) con buildx"
docker buildx build --platform linux/amd64 -t "$FULL_IMAGE" --load .

echo "👉 Login a ACR"
az acr login --name "$ACR"

echo "👉 Push"
docker push "$FULL_IMAGE"

echo "👉 Despliegue a Container Apps"
az containerapp update \
  --name "$APP" \
  --resource-group "$RG" \
  --image "$FULL_IMAGE"

echo "✅ Listo. URL:"
az containerapp show \
  --name "$APP" \
  --resource-group "$RG" \
  --query properties.configuration.ingress.fqdn -o tsv
  
