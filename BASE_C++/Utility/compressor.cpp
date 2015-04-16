#include "compressor.hpp"

Compressor* Compressor::instance = new Compressor();

Compressor::Compressor()
{
    //std::cout << "instanciation de compressor" << std::endl;
}

Compressor *Compressor::getInstance()
{
	return instance;
}

void Compressor::compress(EncByteBuffer &encBB)
{
	char * newBuffer = new char[encBB.getLength()];
	int newLength = LZ4_compress (reinterpret_cast<const char*>(encBB.getData()), newBuffer, encBB.getLength());
    //encBB.setBuffer(reinterpret_cast<unsigned char*>(newBuffer));
    //encBB.setLength(newLength);
    //encBB.setType(1 & encBB.getType());
}
