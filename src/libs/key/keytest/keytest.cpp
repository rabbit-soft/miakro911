// keytest.cpp: определяет точку входа для консольного приложения.
//

#include "stdafx.h"
#include "string.h"
#include "../key.h"

#define CODE_PUB	0x9BD54F75
#define CODE_RD		0xFE8392B2

int findkey()
{
	int res=HasKey(CODE_PUB,CODE_RD);
	printf("haskey result=%d\n",res);
	return 0;
}

int verify()
{
	int res=VerifyKey(CODE_PUB,CODE_RD);
	printf("verify result=%d\n",res);
	return 0;
}


int process(char* cmd)
{
	if (!strcmp(cmd,"find"))
		return findkey();
	if (!strcmp(cmd,"verify"))
		return verify();
	printf("\n");
	return 0;
}


int _tmain(int argc, _TCHAR* argv[])
{
	char cmd[100]={0};
	do
	{
		printf("keytest>");
		scanf("%s",cmd);
		process(cmd);
	}while(strcmp(cmd,"quit") && strcmp(cmd,"exit"));
	return 0;
}

