Проект утилиты поиска существующих файлов.

Утилита должна находить в целевой директории файлы, существующие в исходной.
Например, если имеем вот такие дирректории:
(здесь одинаковый размер подразумевает одинаковое содержимое)
		c:\source\
					 23Kb.txt	23Kb
					 _23Kb.txt	23Kb

					 3Kb.txt	 3Kb

					 11Kb.txt	11Kb
					_11Kb.txt	11Kb

					 9Kb.dat	 9Kb
					_9Kb.dat	 9Kb


		c:\target\
					24Kb.txt	24Kb

					 4Kb.txt	 4Kb

					11Kb.txt	11Kb

					 9Kb.dat	 9Kb

Программа, запущенная вот с такими параметрами:		c:\>DirectoriesComparator.exe /source: c:\source /target: c:\target
должна вывести на экран:

Groups of equal files:
 ---- Group 1
Source equal files:
		c:\source\11Kb.txt
		c:\source\_11Kb.txt

Target equal files:
		c:\target\11Kb.txt

 ---- Group 2
Source equal files:
		c:\source\9Kb.dat
		c:\source\_9Kb.dat
Target equal files:
		c:\target\9Kb.dat
