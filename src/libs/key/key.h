#ifndef _DLL_H_
#define _DLL_H_

#include <windows.h>

#ifdef KEY_EXPORTS
#define KEY_API __declspec(dllexport)
#else
#define KEY_API __declspec(dllimport)
#endif


extern "C" int APIENTRY HasKey(int pub,int rd);
extern "C" int APIENTRY VerifyKey(int pub,int rd);


#endif /* _DLL_H_ */
