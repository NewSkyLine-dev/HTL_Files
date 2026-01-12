#pragma once
class Image
{
public:
	std::string filename;
	int width;
	int height;

	Image() : filename(""), width(0), height(0) { ; }
	Image(const std::string& fn, const int& w, const int& h)
		: filename(fn), width(w), height(h) {
		;
	}
	~Image() { ; }
};

