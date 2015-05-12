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
    /*SharedResourcePtr ptr = ress [0];
    std::cout << "Got ptr" << std::endl;
    QUuid quid = ptr.data()->getUUID();
    std::cout << "quid got" << std::endl;*/
    /*std::cout << "Before getting quid" << std::endl;
    SharedResourceList list = ResourceHolder::GetAllByTypeName("GameEntity");
    std::cout << "list got " << list.size() << std::endl;
    QUuid entity = list[0] ->getUUID();
    std::cout << "entity got" << std::endl;*/

    //std::cout << "QUuid : " << entity.toString().toStdString() << std::endl;
	

    while(true) {
        while(client == fake) {
            client = ServerManager::getInstance()->getConnection()->listen();
        }
        std::cout << "Client found" <<  client.toString().toStdString() << std::endl;
        ServerManager::getInstance()->interpret(client);
        std::cout << "Client disconnected" << std::endl;
        //client = fake;
    }//*/
	
	/*while ( true ) {
        // attente d'un client
        std::cout << "Wait for client" << std::endl;
        QUuid client;
        while ( client == fake ) {
            client = tcp.listen();
        }
        std::cout << "Client " << client.toString().toStdString() << " connected" << std::endl;
        // envoi l'UUID du 1er monde lors de la connexion d'un client
        QUuid world = ResourceHolder::GetAllByTypeName("World" ) [ 0 ]->getUUID();
        EncByteBuffer bworld = ::toBuffer (world);
		bworld.setType(4);
        if ( tcp.send ( client, bworld ) ) {
            unsigned long long sended = 0;
			while(true){
				if(! ServerManager::getInstance()->interpret(client))
					break;
			}
            
			
			/*while ( true ) {
                // attente d'une requete du client
                EncByteBuffer request;
                QUuid uuid;
                unsigned long long index = 0;
                if ( ! server.receive(client,request) )
                    break;
                index = ::fromBuffer(request,index, uuid);
                std::cout << "Request for " << uuid.toString().toStdString() << "..."; std::cout.flush();
                // recuperation de la ressource
                SharedResourcePtr resource = ResourceHolder::GetByUUID(uuid);
                // conversion binaire
                ByteBuffer reply = ResourceHolder::ToBuffer(resource);
                // envoi de la reponse
                if ( ! server.send(client,reply) )
                    break;
                sended += reply.getLength();
                std::cout << " replied" << std::endl;
            }*/
            /*std::cout << "Sent " << sended << " bytes" << std::endl;
        }
        // deconnexion du client
    }*/


    ServerManager::getInstance()->stopConnection();
    return 0;
}
