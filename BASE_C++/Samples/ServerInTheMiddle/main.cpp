#include <unistd.h>

#include "QImageLoader/qimageloader.hpp"
#include "Wavefront/wavefront.hpp"
#include "ServerManager/EncTcpStartPoint.hpp"
#include "ServerManager/ServerManager.hpp"

extern void __attachQImage(void);
extern void __attachWavefront(void);

int main(int argc, char** argv) {

    /*
    __attachQImage();
    __attachWavefront();

    QUuid fake;
    SimpleTcpStartPoint::Options options;
    options.maximumConnectedClients = 200;
    options.connectionPort = 3000;
    EncTcpStartPoint tcp(options);
    ServerManager::getInstance()->linkTcp(tcp);
    /*ServerManager::getInstance()->startConnection();
    QUuid client;

    /* Tant qu'on est actif
     *  Connexion tant que pas de client
     *      Lire le message
     *      Fin du client
     * Fin de la connexion
    */

    /*std::cout << "Loading file" << std::endl;
    FileDescriptor file ( argv[1]);
    std::cout << "Descriptor got" << std::endl;
    SharedResourceList ress = ResourceHolder::Load(file);
    std::cout << "Got list" << std::endl;
    SharedResourcePtr ptr = ress[0];
    std::cout << "Got pointer" << std::endl;
    QUuid quid = ptr.data()->getUUID();

    std::cout << "QUuid : " << quid.toString().toStdString() << std::endl;
    /*
    while(true) {
        while(client == fake) {
            client = ServerManager::getInstance()->getConnection()->listen();
        }
        std::cout << "Client found" <<  client.toString().toStdString() << std::endl;
        ServerManager::getInstance()->interpret(client);
        std::cout << "Client disconnected" << std::endl;
        client = fake;
    }

    ServerManager::getInstance()->stopConnection();*/

    __attachQImage();
    __attachWavefront();

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
    FileDescriptor file ( argv[1]);
    SharedResourceList ress = ResourceHolder::Load(file);
    std::cout << "** Resource loaded" << std::endl;
    SharedResourcePtr ptr = ress [0];
    QUuid quid = ptr.data()->getUUID();

    std::cout << "QUuid : " << quid.toString().toStdString() << std::endl;
    /*std::cout << "Loading file" << std::endl;
        FileDescriptor file ( argv[1]);
        std::cout << "Descriptor got" << std::endl;
        SharedResourceList ress = ResourceHolder::Load(file);
        std::cout << "Got list" << std::endl;
        SharedResourcePtr ptr = ress[0];
        std::cout << "Got pointer" << std::endl;

*/

    while(true) {
        while(client == fake) {
            client = ServerManager::getInstance()->getConnection()->listen();
        }
        std::cout << "Client found" <<  client.toString().toStdString() << std::endl;
        ServerManager::getInstance()->interpret(client);
        std::cout << "Client disconnected" << std::endl;
        //client = fake;
    }

    ServerManager::getInstance()->stopConnection();
    return 0;
}
