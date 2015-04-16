#ifndef COMPRESSOR_H
#define COMPRESSOR_H

#include "Utility/lz4.hpp"
#include "Utility/message.hpp"
//#include "ServerManager/ServerManager.hpp"

class Compressor
{
public:
	void compress(EncByteBuffer &encBB);
	static Compressor* getInstance();
protected:
	static Compressor* instance;
private:
	Compressor();
};

#endif // COMPRESSOR_H