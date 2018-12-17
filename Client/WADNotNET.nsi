; 该脚本使用 HM VNISEdit 脚本编辑器向导产生

; 安装程序初始定义常量
!define PRODUCT_NAME "我的安装程序"
!define PRODUCT_VERSION "1.0"
!define PRODUCT_PUBLISHER "liutao"
!define PRODUCT_WEB_SITE "http://www.mycompany.com"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\WindowsFormsApplication1.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

SetCompressor lzma

; ------ MUI 现代界面定义 (1.67 版本以上兼容) ------
!include "MUI.nsh"

; MUI 预定义常量
!define MUI_ABORTWARNING
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\modern-install.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; 欢迎页面
!insertmacro MUI_PAGE_WELCOME
; 许可协议页面

; 组件选择页面
!insertmacro MUI_PAGE_COMPONENTS
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
OutFile "WADSetup.exe"
InstallDir "$PROGRAMFILES\万安迪气体检测软件安装程序"
InstallDirRegKey HKLM "${PRODUCT_UNINST_KEY}" "UninstallString"
ShowInstDetails show
ShowUnInstDetails show
BrandingText " "

Section "MainSection" SEC01
  SetOutPath "$INSTDIR"
  SetOverwrite ifnewer
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\CommandManager.dll"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\Dal.dll"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DataStruct.dll"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\Entity.dll"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\LibMessageHelper.dll"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\log4net.dll"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\LogLib.dll"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\LogLib.dll.config"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WAD.db3"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\ALARM1.WAV"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\WADApplication.exe"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\WADApplication.exe.Config"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.Charts.v11.2.Core.dll"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.Data.v11.2.dll"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.Data.v11.2.xml"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.Printing.v11.2.Core.dll"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.Printing.v11.2.Core.xml"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.Utils.v11.2.dll"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.Utils.v11.2.xml"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.XtraBars.v11.2.dll"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.XtraBars.v11.2.xml"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.XtraCharts.v11.2.dll"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.XtraCharts.v11.2.xml"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.XtraCharts.v11.2.UI.dll"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.XtraCharts.v11.2.UI.xml"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.XtraEditors.v11.2.dll"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.XtraEditors.v11.2.xml"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.XtraGrid.v11.2.dll"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.XtraGrid.v11.2.xml"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.XtraLayout.v11.2.dll"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.XtraLayout.v11.2.xml"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.XtraRichEdit.v11.2.dll"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.XtraRichEdit.v11.2.xml"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\DevExpress.RichEdit.v11.2.Core.dll"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\Libs\SQLite.Interop.dll"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\Libs\System.Data.SQLite.dll"
  File "E:\公司项目\万安迪\新项目\新建文件夹\客户端Client\WADApplication\WADApplication\bin\Debug\WAD_SkinProject.dll"
  CreateDirectory "$SMPROGRAMS\万安迪气体检测软件"
  CreateShortCut "$SMPROGRAMS\万安迪气体检测软件\气体浓度监测软件.lnk" "$INSTDIR\WADApplication.exe"
  CreateShortCut "$DESKTOP\气体浓度监测软件.lnk" "$INSTDIR\WADApplication.exe"
SectionEnd

Section -AdditionalIcons
  WriteIniStr "$INSTDIR\${PRODUCT_NAME}.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"
  CreateShortCut "$SMPROGRAMS\万安迪气体检测软件\Website.lnk" "$INSTDIR\${PRODUCT_NAME}.url"
  CreateShortCut "$SMPROGRAMS\万安迪气体检测软件\Uninstall.lnk" "$INSTDIR\uninst.exe"
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\WADApplication.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\WADApplication.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd

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
  Delete "$INSTDIR\WADApplication.exe"
  Delete "$INSTDIR\CommandManager.dll"
  Delete "$INSTDIR\Dal.dll"
  Delete "$INSTDIR\DataStruct.dll"
  Delete "$INSTDIR\Entity.dll"
  Delete "$INSTDIR\LibMessageHelper.dll"
  Delete "$INSTDIR\log4net.dll"
  Delete "$INSTDIR\LogLib.dll"
  Delete "$INSTDIR\LogLib.dll.config"
  Delete "$INSTDIR\WAD.accdb"
  Delete "$INSTDIR\ALARM1.WAV"
  Delete "$INSTDIR\WADApplication.exe"
  Delete "$INSTDIR\WADApplication.exe.Config"
  Delete "$INSTDIR\DevExpress.Charts.v11.2.Core.dll"
  Delete "$INSTDIR\DevExpress.Data.v11.2.dll"
  Delete "$INSTDIR\DevExpress.Data.v11.2.xml"
  Delete "$INSTDIR\DevExpress.Printing.v11.2.Core.dll"
  Delete "$INSTDIR\DevExpress.Printing.v11.2.Core.xml"
  Delete "$INSTDIR\DevExpress.Utils.v11.2.dll"
  Delete "$INSTDIR\DevExpress.Utils.v11.2.xml"
  Delete "$INSTDIR\DevExpress.XtraBars.v11.2.dll"
  Delete "$INSTDIR\DevExpress.XtraBars.v11.2.xml"
  Delete "$INSTDIR\DevExpress.XtraCharts.v11.2.dll"
  Delete "$INSTDIR\DevExpress.XtraCharts.v11.2.xml"
  Delete "$INSTDIR\DevExpress.XtraCharts.v11.2.UI.dll"
  Delete "$INSTDIR\DevExpress.XtraCharts.v11.2.UI.xml"
  Delete "$INSTDIR\DevExpress.XtraEditors.v11.2.dll"
  Delete "$INSTDIR\DevExpress.XtraEditors.v11.2.xml"
  Delete "$INSTDIR\DevExpress.XtraGrid.v11.2.dll"
  Delete "$INSTDIR\DevExpress.XtraGrid.v11.2.xml"
  Delete "$INSTDIR\DevExpress.XtraLayout.v11.2.dll"
  Delete "$INSTDIR\DevExpress.XtraLayout.v11.2.xml"
  Delete "$INSTDIR\DevExpress.XtraRichEdit.v11.2.dll"
  Delete "$INSTDIR\DevExpress.XtraRichEdit.v11.2.xml"
  Delete "$INSTDIR\DevExpress.RichEdit.v11.2.Core.dll"
  Delete "$INSTDIR\WAD_SkinProject.dll"

  Delete "$SMPROGRAMS\万安迪气体检测软件\Uninstall.lnk"
  Delete "$SMPROGRAMS\万安迪气体检测软件\Website.lnk"
  Delete "$DESKTOP\气体浓度监测软件.lnk"
  Delete "$SMPROGRAMS\万安迪气体检测软件\气体浓度监测软件.lnk"

  RMDir "$SMPROGRAMS\万安迪气体检测软件"

  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd

#-- 根据 NSIS 脚本编辑规则，所有 Function 区段必须放置在 Section 区段之后编写，以避免安装程序出现未可预知的问题。--#

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "您确实要完全移除 $(^Name) ，及其所有的组件？" IDYES +2
  Abort
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) 已成功地从您的计算机移除。"
FunctionEnd
