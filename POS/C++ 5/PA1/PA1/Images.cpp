#include "stdafx.h"
#include "Images.h"

#include "QxOrm_Impl.h"

QX_REGISTER_CPP_QX_PA1(Images)

namespace qx {
	template <> void register_class(QxClass<Images>& t)
	{
		t.id(&Images::id, "id");

		t.data(&Images::filename, "filename");
		t.data(&Images::width, "width");
		t.data(&Images::height, "height");
	}
}