#include <unistd.h>

#include "QImageLoader/qimageloader.hpp"
#include "Wavefront/wavefront.hpp"
#include "TcpNetworking/simpletcpstartpoint.hpp"
#include "ServerManager/ServerManager.hpp"

int main() {
    QUuid fake;
    ServerManager::getInstance()->startConnection();
    QUuid client;

    /* Tant qu'on est actif
     *  Connexion tant que pas de client
     *      Lire le message
     *      Fin du client
     * Fin de la connexion
    */

    while(client == fake) {
            client = ServerManager::getInstance()->getConnection()->listen();
    }
    while(ServerManager::getInstance()->getConnection()->isStarted())
        ServerManager::getInstance()->interpret(client);

    client = fake;

    ServerManager::getInstance()->stopConnection();
    return 0;
}
