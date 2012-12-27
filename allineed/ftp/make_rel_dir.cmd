@echo off
echo user ccnet_usr> ftpcmd.dat
echo netc,jhobr>> ftpcmd.dat
echo cd ./rabbits>> ftpcmd.dat
echo mkdir rel>> ftpcmd.dat
echo cd ./rel>> ftpcmd.dat
echo mkdir %1>> ftpcmd.dat
echo quit>> ftpcmd.dat
ftp -n -s:ftpcmd.dat 91.219.189.163
del ftpcmd.dat