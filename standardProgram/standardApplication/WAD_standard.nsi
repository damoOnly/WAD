; 该脚本使用 HM VNISEdit 脚本编辑器向导产生

; 安装程序初始定义常量
!define PRODUCT_NAME "控制器标定软件"
!define PRODUCT_VERSION "v2.0"
!define PRODUCT_PUBLISHER "WAD"
!define PRODUCT_WEB_SITE "http://www.mycompany.com"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\standardApplication.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

SetCompressor lzma

; ------ MUI 现代界面定义 (1.67 版本以上兼容) ------
!include "MUI.nsh"

; MUI 预定义常量
!define MUI_ABORTWARNING
!define MUI_ICON "..\wad_favicon_32.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; 欢迎页面
!insertmacro MUI_PAGE_WELCOME
; 许可协议页面

; 组件选择页面
; !insertmacro MUI_PAGE_COMPONENTS
; 安装目录选择页面
!insertmacro MUI_PAGE_DIRECTORY
; 安装过程页面
!insertmacro MUI_PAGE_INSTFILES
; 安装完成页面
!insertmacro MUI_PAGE_FINISH

; 安装卸载过程页面
!insertmacro MUI_UNPAGE_INSTFILES

; 安装界面包含的语言设置
!insertmacro MUI_LANGUAGE "SimpChinese"

; 安装预释放文件
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS
; ------ MUI 现代界面定义结束 ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "WADStandardSetup.exe"
InstallDir "d:\控制器标定软件"
InstallDirRegKey HKLM "${PRODUCT_UNINST_KEY}" "UninstallString"
ShowInstDetails show
ShowUnInstDetails show
BrandingText " "

Section "MainSection" SEC01
  SetOutPath "$INSTDIR"
  SetOverwrite ifnewer
  File "standardApplication\bin\Debug\CommandManager.dll"
  File "standardApplication\bin\Debug\Entity.dll"
  File "standardApplication\bin\Debug\log4net.dll"
  File "standardApplication\bin\Debug\LogLib.dll"
  File "standardApplication\bin\Debug\standardApplication.exe"
  File "standardApplication\bin\Debug\standardApplication.pdb"
  File "standardApplication\bin\Debug\CommandManager.pdb"
  File "standardApplication\bin\Debug\Entity.pdb"
  File "standardApplication\bin\Debug\LogLib.pdb"
  File "standardApplication\bin\Debug\DevExpress.Dashboard.v14.2.Core.dll"
  File "standardApplication\bin\Debug\DevExpress.Data.v14.2.dll"
  File "standardApplication\bin\Debug\DevExpress.DataAccess.v14.2.dll"
  File "standardApplication\bin\Debug\DevExpress.Office.v14.2.Core.dll"
  File "standardApplication\bin\Debug\DevExpress.Printing.v14.2.Core.dll"
  File "standardApplication\bin\Debug\DevExpress.RichEdit.v14.2.Core.dll"
  File "standardApplication\bin\Debug\DevExpress.Utils.v14.2.dll"
  File "standardApplication\bin\Debug\DevExpress.XtraBars.v14.2.dll"
  File "standardApplication\bin\Debug\DevExpress.XtraEditors.v14.2.dll"
  File "standardApplication\bin\Debug\DevExpress.XtraGrid.v14.2.dll"
  File "standardApplication\bin\Debug\DevExpress.XtraLayout.v14.2.dll"
  File "standardApplication\bin\Debug\DevExpress.XtraNavBar.v14.2.dll"
  File "standardApplication\bin\Debug\DevExpress.XtraPrinting.v14.2.dll"
  File "standardApplication\bin\Debug\DevExpress.XtraRichEdit.v14.2.dll"
  File "standardApplication\bin\Debug\DevExpress.XtraTreeList.v14.2.dll"
  File "standardApplication\bin\Debug\DevExpress.XtraVerticalGrid.v14.2.dll"
  File "standardApplication\CommonConfig.xml"
  CreateDirectory "$SMPROGRAMS\控制器标定软件"
  CreateShortCut "$SMPROGRAMS\控制器标定软件\控制器标定软件.lnk" "$INSTDIR\standardApplication.exe"
  CreateShortCut "$DESKTOP\控制器标定软件.lnk" "$INSTDIR\standardApplication.exe"
SectionEnd

Section -AdditionalIcons
  WriteIniStr "$INSTDIR\${PRODUCT_NAME}.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"
  CreateShortCut "$SMPROGRAMS\控制器标定软件\Website.lnk" "$INSTDIR\${PRODUCT_NAME}.url"
  CreateShortCut "$SMPROGRAMS\控制器标定软件\Uninstall.lnk" "$INSTDIR\uninst.exe"
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\standardApplication.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\standardApplication.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd

; 区段组件描述
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  !insertmacro MUI_DESCRIPTION_TEXT ${SEC01} ""
!insertmacro MUI_FUNCTION_DESCRIPTION_END

/******************************
 *  以下是安装程序的卸载部分  *
 ******************************/

Section Uninstall
  Delete "$INSTDIR\${PRODUCT_NAME}.url"
  Delete "$INSTDIR\uninst.exe"
  Delete "$INSTDIR\standardApplication.exe"
  Delete "$INSTDIR\CommandManager.dll"
  Delete "$INSTDIR\Entity.dll"
  Delete "$INSTDIR\log4net.dll"
  Delete "$INSTDIR\LogLib.dll"
  Delete "$INSTDIR\DevExpress.Dashboard.v14.2.Core.dll"
  Delete "$INSTDIR\DevExpress.Data.v14.2.dll"
  Delete "$INSTDIR\DevExpress.DataAccess.v14.2.dll"
  Delete "$INSTDIR\DevExpress.Office.v14.2.Core.dll"
  Delete "$INSTDIR\DevExpress.Printing.v14.2.Core.dll"
  Delete "$INSTDIR\DevExpress.RichEdit.v14.2.Core.dll"
  Delete "$INSTDIR\DevExpress.Utils.v14.2.dll"
  Delete "$INSTDIR\DevExpress.XtraBars.v14.2.dll"
  Delete "$INSTDIR\DevExpress.XtraEditors.v14.2.dll"
  Delete "$INSTDIR\DevExpress.XtraGrid.v14.2.dll"
  Delete "$INSTDIR\DevExpress.XtraLayout.v14.2.dll"
  Delete "$INSTDIR\DevExpress.XtraNavBar.v14.2.dll"
  Delete "$INSTDIR\DevExpress.XtraPrinting.v14.2.dll"
  Delete "$INSTDIR\DevExpress.XtraRichEdit.v14.2.dll"
  Delete "$INSTDIR\DevExpress.XtraTreeList.v14.2.dll"
  Delete "$INSTDIR\DevExpress.XtraVerticalGrid.v14.2.dll"
  Delete "$INSTDIR\CommonConfig.xml"

  Delete "$SMPROGRAMS\控制器标定软件\Uninstall.lnk"
  Delete "$SMPROGRAMS\控制器标定软件\Website.lnk"
  Delete "$DESKTOP\控制器标定软件.lnk"
  Delete "$SMPROGRAMS\控制器标定软件\控制器标定软件.lnk"

  RMDir "$SMPROGRAMS\控制器标定软件"

  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd

#-- 根据 NSIS 脚本编辑规则，所有 Function 区段必须放置在 Section 区段之后编写，以避免安装程序出现未可预知的问题。--#

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "您确定要完全移除 $(^Name) ，及其所有的组件？" IDYES +2
  Abort
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) 已成功地从您的计算机移除。"
FunctionEnd


#-- 根据 NSIS 脚本编辑规则，所有 Function 区段必须放置在 Section 区段之后编写，以避免安装程序出现未可预知的问题。--#
Function GetNetFrameworkVersion
#--;获取.Net Framework版本支持--#
Push $1
Push $0
ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Install"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Version"
StrCmp $0 1 KnowNetFrameworkVersion +1
ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5" "Install"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5" "Version"
StrCmp $0 1 KnowNetFrameworkVersion +1
ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.0\Setup" "InstallSuccess"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.0\Setup" "Version"
StrCmp $0 1 KnowNetFrameworkVersion +1
ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727" "Install"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727" "Version"
StrCmp $1 "" +1 +2
StrCpy $1 "2.0.50727.832"
StrCmp $0 1 KnowNetFrameworkVersion +1
ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v1.1.4322" "Install"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v1.1.4322" "Version"
StrCmp $1 "" +1 +2
StrCpy $1 "1.1.4322.573"
StrCmp $0 1 KnowNetFrameworkVersion +1
ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\.NETFramework\policy\v1.0" "Install"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\.NETFramework\policy\v1.0" "Version"
StrCmp $1 "" +1 +2
StrCpy $1 "1.0.3705.0"
StrCmp $0 1 KnowNetFrameworkVersion +1
StrCpy $1 "not .NetFramework"
KnowNetFrameworkVersion:
Pop $0
Exch $1
FunctionEnd
