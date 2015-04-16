#ifndef ENC_TCP_STARTPOINT
#define ENC_TCP_STARTPOINT
#include "TcpNetworking/simpletcpstartpoint.hpp"
#include "Utility/message.hpp"

class EncTcpStartPoint : virtual public SimpleTcpStartPoint
{
    public:
        EncTcpStartPoint(ConnectionStartPoint::Options& options);
        bool send(QUuid client, const EncByteBuffer& buffer);
        bool receive(QUuid client, EncByteBuffer& buffer);
};

#endif
