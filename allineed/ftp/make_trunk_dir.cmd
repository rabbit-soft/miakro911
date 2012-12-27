@echo off
echo user ccnet_usr> ftpcmd.dat
echo netc,jhobr>> ftpcmd.dat
echo cd ./rabbits>> ftpcmd.dat
echo mkdir trunk>> ftpcmd.dat
echo quit>> ftpcmd.dat
ftp -n -s:ftpcmd.dat 91.219.189.163
del ftpcmd.dat