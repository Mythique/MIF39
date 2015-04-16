#include <unistd.h>

#include "QImageLoader/qimageloader.hpp"
#include "Wavefront/wavefront.hpp"
#include "ServerManager/EncTcpStartPoint.hpp"
#include "ServerManager/ServerManager.hpp"

int main() {
    QUuid fake;
    SimpleTcpStartPoint::Options options;
    options.maximumConnectedClients = 200;
    options.connectionPort = 3000;
    EncTcpStartPoint tcp(options);
    ServerManager::getInstance()->linkTcp(tcp);
    ServerManager::getInstance()->startConnection();
    QUuid client;

    /* Tant qu'on est actif
     *  Connexion tant que pas de client
     *      Lire le message
     *      Fin du client
     * Fin de la connexion
    */

    while(true) {
        while(client == fake) {
            client = ServerManager::getInstance()->getConnection()->listen();
        }
        std::cout << "Client found" <<  client.toString().toStdString() << std::endl;
        ServerManager::getInstance()->interpret(client);
        std::cout << "Client disconnected" << std::endl;
        client = fake;
    }

    ServerManager::getInstance()->stopConnection();
    return 0;
}
