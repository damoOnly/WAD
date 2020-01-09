; �ýű�ʹ�� HM VNISEdit �ű��༭���򵼲���

; ��װ�����ʼ���峣��
!define PRODUCT_NAME "�������궨���"
!define PRODUCT_VERSION "v2.0"
!define PRODUCT_PUBLISHER "WAD"
!define PRODUCT_WEB_SITE "http://www.mycompany.com"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\standardApplication.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

SetCompressor lzma

; ------ MUI �ִ����涨�� (1.67 �汾���ϼ���) ------
!include "MUI.nsh"

; MUI Ԥ���峣��
!define MUI_ABORTWARNING
!define MUI_ICON "..\wad_favicon_32.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; ��ӭҳ��
!insertmacro MUI_PAGE_WELCOME
; ���Э��ҳ��

; ���ѡ��ҳ��
; !insertmacro MUI_PAGE_COMPONENTS
; ��װĿ¼ѡ��ҳ��
!insertmacro MUI_PAGE_DIRECTORY
; ��װ����ҳ��
!insertmacro MUI_PAGE_INSTFILES
; ��װ���ҳ��
!insertmacro MUI_PAGE_FINISH

; ��װж�ع���ҳ��
!insertmacro MUI_UNPAGE_INSTFILES

; ��װ�����������������
!insertmacro MUI_LANGUAGE "SimpChinese"

; ��װԤ�ͷ��ļ�
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS
; ------ MUI �ִ����涨����� ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "WADStandardSetup.exe"
InstallDir "d:\�������궨���"
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
  CreateDirectory "$SMPROGRAMS\�������궨���"
  CreateShortCut "$SMPROGRAMS\�������궨���\�������궨���.lnk" "$INSTDIR\standardApplication.exe"
  CreateShortCut "$DESKTOP\�������궨���.lnk" "$INSTDIR\standardApplication.exe"
SectionEnd

Section -AdditionalIcons
  WriteIniStr "$INSTDIR\${PRODUCT_NAME}.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"
  CreateShortCut "$SMPROGRAMS\�������궨���\Website.lnk" "$INSTDIR\${PRODUCT_NAME}.url"
  CreateShortCut "$SMPROGRAMS\�������궨���\Uninstall.lnk" "$INSTDIR\uninst.exe"
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

; �����������
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  !insertmacro MUI_DESCRIPTION_TEXT ${SEC01} ""
!insertmacro MUI_FUNCTION_DESCRIPTION_END

/******************************
 *  �����ǰ�װ�����ж�ز���  *
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

  Delete "$SMPROGRAMS\�������궨���\Uninstall.lnk"
  Delete "$SMPROGRAMS\�������궨���\Website.lnk"
  Delete "$DESKTOP\�������궨���.lnk"
  Delete "$SMPROGRAMS\�������궨���\�������궨���.lnk"

  RMDir "$SMPROGRAMS\�������궨���"

  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd

#-- ���� NSIS �ű��༭�������� Function ���α�������� Section ����֮���д���Ա��ⰲװ�������δ��Ԥ֪�����⡣--#

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "��ȷ��Ҫ��ȫ�Ƴ� $(^Name) ���������е������" IDYES +2
  Abort
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) �ѳɹ��ش����ļ�����Ƴ���"
FunctionEnd


#-- ���� NSIS �ű��༭�������� Function ���α�������� Section ����֮���д���Ա��ⰲװ�������δ��Ԥ֪�����⡣--#
Function GetNetFrameworkVersion
#--;��ȡ.Net Framework�汾֧��--#
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
