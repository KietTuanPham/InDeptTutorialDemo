;#DropDB InDeptTutorialDemoEasyTest

#Application InDeptTutorialDemoWin
#Application InDeptTutorialDemoWeb
#IfDef InDeptTutorialDemoWin

*Action About...
 
*Action OK

#EndIf

#IfDef InDeptTutorialDemoWeb
*Action Navigation(My Details)
#EndIf
