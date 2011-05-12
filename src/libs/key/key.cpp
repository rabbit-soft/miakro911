/* Replace "dll.h" with the name of your header */
#include "key.h"
#include <GrdApi.h>


HANDLE create()
{
	HANDLE h=NULL;
    return GrdCreateHandle(h,GrdCHM_MultiThread,NULL);
}

void destroy(HANDLE h)
{
    GrdLogout(h,0);
    GrdCloseHandle(h);
}

int findkey(HANDLE h,int pub,int rd,int wr,int mst)
{
    int r=-1;
    DWORD id=0;
    int res=GrdSetAccessCodes(h,pub,rd,wr,mst);
    if (res=GrdE_OK) 
        res=GrdSetFindMode(h,GrdFMR_Local,GrdFM_Type,0,0,0,0,0,GrdDT_Win,GrdFMM_ALL,GrdFMI_USB);
    if (res==GrdE_OK)
        res=GrdFind(h,GrdF_First,&id,NULL);
    if (res==GrdE_OK)
        r=(int)id;
    return r;
}

extern "C" int APIENTRY HasKey(int pub,int rd)
{
    HANDLE h=create();
    int i=findkey(h,pub,rd,0,0);
    destroy(h);
    return i;
}

extern "C" int APIENTRY VerifyKey(int pub,int rd)
{
	HANDLE h=create();
	int rs=findkey(h,pub,rd,0,0);
	int res=-1;
	if (rs!=-1)
		rs=GrdLogin(h,0,0);
	if (rs==GrdE_OK)
	{
		char st[8]={0};
		rs=GrdPI_Read(h,12,0,8,st,0,NULL);
		if (rs==GrdE_OK)
		{
			char x[5]={0};
			unsigned int fms=0;
			memcpy(x,st,4);
			memcpy(&fms,&st[4],4);
			if (!strcmp(x,"TBRB"))
			{
				res=fms;
			}else res=-3;
		}else res=rs;
		GrdLogout(h,0);
	}
	destroy(h);
	return res;
}

BOOL APIENTRY DllMain (HINSTANCE hInst     /* Library instance handle. */ ,
                       DWORD reason        /* Reason this function is being called. */ ,
                       LPVOID reserved     /* Not used. */ )
{
	GrdDllMain(hInst,reason,reserved);
    switch (reason)
    {
      case DLL_PROCESS_ATTACH:
            GrdStartup(GrdFMR_Local);
        break;

      case DLL_PROCESS_DETACH:
            GrdCleanup();
        break;

      case DLL_THREAD_ATTACH:
        break;

      case DLL_THREAD_DETACH:
        break;
    }

    /* Returns TRUE on success, FALSE on failure */
    return TRUE;
}

