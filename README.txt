Для запуска программы необходимо пройти в CMD\bin\debug\net5.0, далее написать cmd <путь к директории>

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