#include <unistd.h>

#include "QImageLoader/qimageloader.hpp"
#include "Wavefront/wavefront.hpp"
#include "ServerManager/EncTcpStartPoint.hpp"
#include "ServerManager/ServerManager.hpp"

extern void __attachQImage(void);
extern void __attachWavefront(void);

int main(int argc, char** argv) {

    __attachQImage();
    __attachWavefront();

    QUuid fake;
    SimpleTcpStartPoint::Options options;
    options.maximumConnectedClients = 200;
    options.cbDisconnect = NULL;
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
    ResourceHolder::Import(argv[1]);
    std::cout << "** Resource loaded : " << ResourceHolder::AllKeys ().size () << std::endl;
    std::cout << "Before getting quid" << std::endl;
    SharedResourceList list = ResourceHolder::GetAllByTypeName("Mesh");
    std::cout << "list got " << list.size() << std::endl;
    QUuid mesh = list[0] ->getUUID();
    std::cout << "entity got" << std::endl;
    QUuid world = ResourceHolder::GetAllByTypeName("World")[0]->getUUID();
    EncByteBuffer bworld = ::toBuffer(world);
    bworld.setType(ServerManager::WORLD);

	SharedResourceList listGa = ResourceHolder::GetAllByTypeName("GameObject");
	QUuid gobj = listGa[0] ->getUUID();
	std::cout << "GameObject : " << gobj.toString().toStdString() << std::endl;

    std::cout << "QUuid : " << mesh.toString().toStdString() << std::endl;
	

    while(true) {
        while(client == fake) {
            client = ServerManager::getInstance()->getConnection()->listen();
        }
        if(ServerManager::getInstance()->getConnection()->send(client,bworld)) {
            while(true){
                if(!ServerManager::getInstance()->interpret(client)){
                    client = fake;
                    std::cout << "Effacement du client" << std::endl;
                    break;
                }
            }
        }
        else {
            std::cout << "Erreur, échec de l'envoi du monde" << std::endl;
        }
    }

    ServerManager::getInstance()->stopConnection();
    return 0;
}
