<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
  <meta http-equiv="content-type" content="text/html; charset=iso-8859-1">
  <title>QxOrm : C++ Qt ORM Object Relational Mapping database library - QxEntityEditor : C++ Qt entities graphic editor (data model designer and source code generator)</title>
  <link rel='stylesheet' type='text/css' href='./resource/qxorm_style.css'>
  <script type="text/javascript" src="./resource/jquery.min.js"></script>
  <script type="text/javascript" src="./resource/qxorm_script.js"></script>
</head>
<body>
<table border="0" style="width: 80%" align="center">
  <col><col><col><col><col>
  <tbody>
    <tr>
      <td><a href="./home.html"><img alt="QxOrm" src="./resource/logo_qxorm_and_qxee.png"align="left" border="0"></a></td>
      <td align="right" style="vertical-align:bottom"><div id="qx_search"><gcse:search></gcse:search></div></td>
      <td width="15"></td>
      <td align="right" style="vertical-align:bottom">
        <img alt="Windows" src="./resource/logo_windows.gif" width="35" height="35">
        <img alt="Linux" src="./resource/logo_linux.gif" width="35" height="35">
        <img alt="Macintosh" src="./resource/logo_mac.gif" width="35" height="35">
      </td>
      <td width="70"><img alt="C++" src="./resource/logo_cpp.gif" width="50" height="50" align="right"></td>
    </tr>
  </tbody>
</table>
<hr style="width: 80%" align="center" size="1" color="#100D5A">
<table border="0" style="width: 80%" align="center">
  <col><col><col><col><col><col>
  <tbody>
    <tr>
      <td align="center"><a href="./home.html" class="btn_nav">Accueil</a></td>
      <td align="center"><a href="./download.html" class="btn_nav">T�l�chargement</a></td>
      <td align="center"><a href="./quick_sample.html" class="btn_nav">Exemple rapide</a></td>
      <td align="center" onmouseover="showHideElementById('menu_tuto', true);" onmouseout="showHideElementById('menu_tuto', false);">
         <a href="./tutorial.html" class="btn_nav">Tutoriel (4)</a>
         <table class="table_menu_tuto"><tbody><tr><td>
            <div id="menu_tuto" class="div_menu_tuto">
               <a href="./tutorial_3.html" class="btn_sub_menu">install QxOrm</a><br/>
               <a href="./tutorial.html" class="btn_sub_menu">qxBlog</a><br/>
               <a href="./tutorial_2.html" class="btn_sub_menu">qxClientServer</a><br/>
               <a href="./tutorial_4.html" class="btn_sub_menu">QxEntityEditor videos</a>
            </div>
         </td></tr></tbody></table>
      </td>
      <td align="center" onmouseover="showHideElementById('menu_manual', true);" onmouseout="showHideElementById('menu_manual', false);">
         <a href="./manual.html" class="btn_nav">Manuel (2)</a>
         <table class="table_menu_manual"><tbody><tr><td>
            <div id="menu_manual" class="div_menu_manual">
               <a href="./manual.html" class="btn_sub_menu">Manuel QxOrm</a><br/>
               <a href="./manual_qxee.html" class="btn_sub_menu">Manuel QxEntityEditor</a><br/>
            </div>
         </td></tr></tbody></table>
      </td>
      <td align="center"><a href="./link.html" class="btn_nav">Forum</a></td>
      <td align="center"><a href="./customer.html" class="btn_nav">Nos clients</a></td>
    </tr>
  </tbody>
</table>
<hr style="width: 80%" align="center" size="1" color="#100D5A">
<table border="0" style="width: 80%" align="center">
  <col><col><col><col><col><col>
  <tbody><tr>
  <td align="left" valign="top"><font size="2" class="txt_with_shadow">QxOrm  &gt;&gt;  Informations sur le t�l�chargement</font></td>
  <td align="right" valign="top">
    <table cellspacing="0" cellpadding="1"><col><col><tbody>
      <tr>
         <td align="right" valign="top"><font size="2" class="txt_with_shadow">Version courante :&nbsp;</font></td>
         <td align="left" valign="top"><font size="2" class="txt_with_shadow">QxOrm 1.5.0 - <a href="../doxygen/index.html" target="_blank">documentation en ligne de la biblioth�que QxOrm</a> - <a href="https://github.com/QxOrm/QxOrm" target="_blank">GitHub</a></font></td>
      </tr>
      <tr>
         <td align="right" valign="top"><font size="2" class="txt_with_shadow"></font></td>
         <td align="left" valign="top"><font size="2" class="txt_with_shadow">QxEntityEditor 1.2.8</font></td>
      </tr>
    </tbody></table>
  </td>
  <td width="10px"></td>
  <td width="40px" height="30px"><a href="../qxorm_fr/download_details.php"><img alt="Version fran�aise du site" src="./resource/FR.png" width="40" height="30" border="0"></a></td>
  <td width="40px" height="30px"><a href="../qxorm_en/download_details.php"><img alt="Web site english version" src="./resource/GB.png" width="40" height="30" border="0"></a></td>
  <td width="40px" height="30px"><a href="http://sites.google.com/site/qxormpostgres/" target="_blank"><img alt="" src="./resource/ES.png" width="40" height="30" border="0"></a></td>
  </tr></tbody>
</table>
<table border="1" frame="vsides" rules="cols" style="width: 80%" align="center" cellpadding="6" bgcolor="#F2F2F4">
  <col>
  <tbody>
    <tr>
      <td align="justify">
         <table border="0" style="width: 100%" align="center">
           <col><col>
           <tbody>
             <tr>
               <td>La biblioth�que QxOrm est disponible avec son code source et peut �tre utilis�e gratuitement en respectant les termes de la licence <a href="./resource/license.gpl3.txt" target="_blank">GNU General Public License (GPL) version 3</a>.
               <br><br>
               En associant (<i>link</i>) une application avec la biblioth�que QxOrm (directement ou indirectement, statiquement ou dynamiquement, � la compilation ou � l'ex�cution), votre application est soumise aux termes de la licence <b>GPLv3</b> 
               qui demande � ce que vous publiez le code source de votre application si et quand vous la distribuez. Distribuer une application signifie la donner ou vendre � des clients, 
               des soci�t�s, filiales, ou tous autres entit�s l�gales diff�rentes de la votre. D'un autre c�t�, lorsque vous utilisez votre application dans votre organistation uniquement, 
               alors vous n'avez pas besoin de rendre votre code source public.
               <br><br>
               Si vous ne souhaitez pas �tre limit� par les termes de la licence GPLv3, vous pouvez acqu�rir la licence <b>QxOrm Proprietary License</b> (<b>QXPL</b>).<br>
               Les avantages de la licence <b>QxOrm Proprietary License</b> sont :
               <ul>
                  <li>Le code source de votre application reste priv� ;</li>
                  <li>Compatible avec toutes les autres licences commerciales et open-source ;</li>
                  <li>Possibilit� d'utiliser QxOrm sans livrer le code source de la biblioth�que aux utilisateurs ;</li>
                  <li>Possibilit� de conserver vos propres modifications � la biblioth�que QxOrm sans les d�voiler ;</li>
                  <li>Possibilit� de cr�er des produits sans mentionner QxOrm aux utilisateurs ;</li>
                  <li>Protection contre le <i>reverse engineering</i> du produit.</li>
               </ul>
               </td>
               <td width="200" align="center" valign="top"><a href="./resource/qt_ambassador_logo.png" target="_blank"><img alt="qt_ambassador" src="./resource/qt_ambassador_logo_150x150.png" width="150" height="150" border="0"></a><br>
                 <b><font size="2">QxOrm library has been accepted into the <a href="http://forum.qt.io/category/24/qt-ambassador-program" target="_blank">Qt Ambassador Program</a></font></b>
               </td>
             </tr>
           </tbody>
         </table>
         La licence QXPL de la biblioth�que QxOrm est une licence par projet (ou par application), peu importe le nombre de d�veloppeurs travaillant sur le projet, et peu importe le nombre d'instances d�ploy�es de votre logiciel (royalty-free).
         Par exemple, si vous d�veloppez 3 applications diff�rentes bas�es sur la biblioth�que QxOrm, alors vous devez acqu�rir 3 licences QXPL (une par projet).<br/>
         <br/>
         Prix d'une licence <b>QxOrm Proprietary License</b> : <i>400&euro;</i>.<br/>
         <br/>
         Pour toutes questions ou pr�cisions sur les licences, ou bien pour acqu�rir la licence <b>QXPL</b>, vous pouvez nous contacter � l'adresse suivante : <a href="mailto:contact@qxorm.com">contact@qxorm.com</a>.<br/>
         <br/>
         <a href="../php/add_download.php5?file_path=QxOrm_1.5.0.zip&file_desc=QxOrm_1.5.0" class="btn_tuto" target="_blank">T�l�charger QxOrm 1.5.0</a>
         <br/>
         <!--
         <script type="text/javascript">
function checkForm(f) { f.submit(); return true; }
         </script>
         <form action="../php/add_download.php" method="POST" onSubmit="return checkForm(this);">
            Votre nom ou le nom de votre soci�t� (optionnel) :<br/>
            <input type="text" id="created_by" name="created_by" size="50" maxlength="200" /><br/>
            Aidez nous � am�liorer la biblioth�que QxOrm en ajoutant vos remarques et suggestions (optionnel) :<br/>
            <textarea id="message_text" name="message_text" rows="3" cols="50"></textarea><br/>
            <input type="submit" value="            T�l�charger QxOrm 1.5.0            " />
         </form>
         -->
         <br>
      </td>
    </tr>
  </tbody>
</table>
<br><hr style="width: 80%" align="center" size="1" color="#100D5A">
<table border="0" style="width: 80%" align="center">
  <col><col><col>
  <tbody>
    <tr>
      <td align="left" valign="middle">
        <img alt="QxOrm" src="./resource/logo_qxorm_small.png" width="168" height="40">
      </td>
      <td align="center" valign="middle">
        <font size="2">� 2011-2024 Lionel Marty - <a href="mailto:contact@qxorm.com">contact@qxorm.com</a></font>
      </td>
      <td align="right" valign="middle">
        <form action="https://www.paypal.com/cgi-bin/webscr" method="post">
           <input type="hidden" name="cmd" value="_s-xclick">
           <input type="hidden" name="hosted_button_id" value="2K4Z58ZYAYJ6S">
           <input type="image" src="./resource/paypal_support_qxorm_library.gif" border="0" name="submit" alt="Support QxOrm library - PayPal">
           <img alt="" border="0" src="https://www.paypalobjects.com/fr_FR/i/scr/pixel.gif" width="1" height="1">
        </form>
      </td>
    </tr>
  </tbody>
</table>
</body>
</html>
