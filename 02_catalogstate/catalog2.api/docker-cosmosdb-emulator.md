# Installer CosmosDB Emulator dans un container docker WSL

Démarrer le container :
```sh
docker pull mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator

ipaddr="`ifconfig | grep "inet " | grep -Fv 127.0.0.1 | awk '{print $2}' | head -n 1`"

docker run -p 8081:8081 -p 10251:10251 -p 10252:10252 -p 10253:10253 -p 10254:10254 \
    -m 3g --cpus=2.0 --name=cosmosdb-emulator \
    -e AZURE_COSMOS_EMULATOR_PARTITION_COUNT=10 \
    -e AZURE_COSMOS_EMULATOR_ENABLE_DATA_PERSISTENCE=true \
    -e AZURE_COSMOS_EMULATOR_IP_ADDRESS_OVERRIDE=$ipaddr \
    -d mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator
```

Quand le container est démarré ( "Started" ) , il faut ajouter le certificat SSL autogénér de l'émulateur dans la liste des certificats de confiance.

Dans un autre shell WSL, Executez  une seule fois (apres le premier démarrage de l'emulateur) 
```sh
ipaddr="`ifconfig | grep "inet " | grep -Fv 127.0.0.1 | awk '{print $2}' | head -n 1`"

curl -k https://$ipaddr:8081/_explorer/emulator.pem > ~/cosmosdbemulatorcert.crt

sudo cp ~/cosmosdbemulatorcert.crt /usr/local/share/ca-certificates/

sudo update-ca-certificates
```

Pour les applications Java, il faut l'importer dans le 'Java  Trusted store'.
```sh
keytool -keystore ~/cacerts -importcert -alias  emulator_cert -file ~/cosmosdbemulatorcert.crt
java -ea -Djavax.net.ssl.trustStore=~/cacerts -Djavax.net.ssl.trustStorePassword="PASSWORD_TO_CHANGE" $APPLICATION_ARGUMENTS
```

Le Data Explorer est accessible sur l'url : https://localhost:8081/_explorer/index.html 