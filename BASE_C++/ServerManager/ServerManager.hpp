#ifndef SERVEURINTER_H
#define SERVEURINTER_H

#include <iostream>
#include "AssetInterfaces/resourceholder.hpp"
#include <Utility/message.hpp>
//#include "TcpNetworking/simpletcpstartpoint.hpp"
#include "ServerManager/EncTcpStartPoint.hpp"

class ServerManager
{
public :
    enum dataType {COMPRESSED=1, SHARED_R=2, WORLD = 4, ERROR = 8, TEST = -1};
    void writeToFile(EncByteBuffer& b);
    static ServerManager* getInstance();
    SharedResourcePtr getRessource(QUuid);
    //bool configure(SimpleTcpStartPoint::Options& option);
    bool linkTcp(EncTcpStartPoint& tcp);
    bool startConnection();
    bool stopConnection();
    EncTcpStartPoint* getConnection();
    bool interpret(QUuid);



protected :
    static bool written;
    static ServerManager* instance;
    EncTcpStartPoint* connection;

private:
    ServerManager();



};

#endif // SERVEURINTER_H
