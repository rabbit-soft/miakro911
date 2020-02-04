[Скачать программу](https://github.com/rabbit-soft/miakro911/releases/download/1.4.2-1-g11124b0/miakro911.zip)

# для разработки необходимы:
	.NET Framework 3.5
	
# сборка

## необходимо установить

- nant ^0.92 <http://nant.sourceforge.net/>
- .NET Framework 3.5 SDK <https://www.microsoft.com/en-us/download/details.aspx?id=3138>
- Eazfuscator.NET 3.1
- NSIS
	
## подготовка к сборке
	
- запустите ./allineed/allineed_instal.cmd
- отредактируйте пути к компонентам в файле ../host_spec.include
- скопируйте файлы из ./allineed/NSIS/ в папку c:\Program Files (x86)\NSIS\ {может отличаться от вашего}
- запустите команду ./nant для сборки проекта	
	
# info

При изменении структуры базы данных (mia_conv), не забудь добавить в Updater скрипт обновления уже существующих баз данных клиентов
