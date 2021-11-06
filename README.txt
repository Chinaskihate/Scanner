Для запуска серверной части необходимо пройти в API\bin\debug\net5.0, далее написать API.
(При запуске из Visual Studio необходимо сменить IIS Express на API).

Для запуска клиентской части необходимо пройти в CMD\bin\debug\net5.0, далее написать cmd <путь к директории>

Команды в клиентской части:
scan <path to directory>
status <scan id>

Программа параллельно сканирует файлы на подозрительные строки.
В процессе она выводит текущий сканируемый файл.
По завершению, программа выводит отчет вида:
====== SCAN RESULT ======
Processed files: <Total processed files>
JS detects: <Total evil javascripts detects>
rm -rf detects: <Total rm -rf detects>
Rundll32 detects: <Total RunDLL detects>
Errors: <Total errors>
Error messages:
	<List of occcured errors>
Execution time: <Execution time>