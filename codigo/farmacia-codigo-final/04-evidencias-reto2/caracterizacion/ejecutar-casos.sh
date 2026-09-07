#!/usr/bin/env bash
# Ejecuta los casos de caracterizacion del Reto 2 redirigiendo entrada y salida.
# Uso: ./ejecutar-casos.sh <as-is|tobe>
set -u

DESTINO="${1:-as-is}"
RAIZ="$(cd "$(dirname "$0")/../.." && pwd)"
ENTRADAS="$RAIZ/04-evidencias-reto2/caracterizacion/entradas"
SALIDAS="$RAIZ/04-evidencias-reto2/caracterizacion/$DESTINO"
APP="$RAIZ/AppFarmaciaConsola"

mkdir -p "$SALIDAS"

dotnet build "$RAIZ/SolucionFarmacia.sln" -c Release >/dev/null || exit 1
EJEC="$APP/bin/Release/net8.0"

for entrada in "$ENTRADAS"/*.in; do
    caso="$(basename "$entrada" .in)"
    # Cada caso arranca con una copia limpia de los .txt: el estado en memoria
    # no se persiste, pero asi se garantiza aislamiento total entre casos.
    cp "$APP"/*.txt "$EJEC"/
    (cd "$EJEC" && dotnet AppFarmaciaConsola.dll < "$entrada" > "$SALIDAS/$caso.out" 2>&1)
    echo "$caso -> $SALIDAS/$caso.out"
done
