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
	int pid;

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

    std::cout << "QUuid : " << mesh.toString().toStdString() << std::endl;
	

    while(true) {
        while(client == fake) {
            client = ServerManager::getInstance()->getConnection()->listen();
        }
        
		pid = -1;
		pid = fork();
		if(pid == 0){

			while(true){

				if(!ServerManager::getInstance()->interpret(client)){
            		
					std::cout << "Déconnexion du client" << std::endl;
					exit(0);
				}
			}
        }
		else if(pid == -1){
			std::cout << "Alerte : erreur lors du Fork. Cient non traité" << std::endl;
		}
		else
			client == fake;
    }

    ServerManager::getInstance()->stopConnection();
    return 0;
}
