<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="OSUDental.index" %>

<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <title>OSU Dental</title>
  <link rel="stylesheet" href="css/bootstrap.min.css">
  <link rel="stylesheet" href="css/ng-grid.css">
  <link rel="stylesheet" href="css/ui-lightness/jquery-ui-1.10.2.custom.min.css">
  <link rel="stylesheet" href="css/app.css">
    <!--[if lt IE 9]>
        <script src="js/es5-shim.min.js"></script>
        <script src="js/angular-ui-ieshiv.min.js"></script>
    <![endif]-->
  <!--<script src="//ajax.googleapis.com/ajax/libs/jquery/1/jquery.min.js"></script>-->
  <script src="js/jquery-1.9.1.min.js"></script>
  <script src="js/jquery-ui-1.10.2.custom.min.js"></script>
<!--  <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>-->
  <script src="js/angular.min.js"></script>
  <script src="js/angular-resource.min.js"></script>
  <script src="js/angular-cookies.min.js"></script>
<!--  <script src="//ajax.googleapis.com/ajax/libs/angularjs/1.0.7/angular.min.js"></script>
  <script src="//ajax.googleapis.com/ajax/libs/angularjs/1.0.7/angular-resource.js"></script>
  <script src="//ajax.googleapis.com/ajax/libs/angularjs/1.0.7/angular-cookies.js"></script>-->
  <script src="js/angular-ui.min.js"></script>
  <script src="js/angular-ui-router.min.js"></script>
  <script src="js/ng-grid-2.0.4.debug.js"></script>
  <script src="js/app.js"></script>
  <script src="js/routingConfig.js"></script>
  <script src="js/controllers.js"></script>
  <script src="js/filters.js"></script>
  <script src="js/services.js"></script>
	<title>College of Dentistry - The Ohio State University</title>
	<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
	<meta name="robots" content="all">
	<meta name="keywords" content="Dentistry, Dental Hygiene, dentist, hygenist">
	<meta name="description" content="The Ohio State University College of Dentistry provides comprehensive patient care in an unparalledled learning environment with internationally recognized faculty and a suportive network of alumni and friends.">
	
	<style type="text/css">
		<!--
@import url(/dent.osu.edu/css/navbar.css);
@import url("/dent.osu.edu/css/dentistry-style.css");
		.skippagenav
{
	position: absolute;
	top: -200px;
	left: -200px;
	z-index: 3;
}

.hidden { display: none; }
	    legend {
	        margin-bottom: 0px;
	    }
		-->
	</style>
</head>
    <body>
<a name="topofpage"></a>
<!--Jumps past page's navigation--><div class="skippagenav"><a href="#pagecontent">Skip OSU and Dentistry navigation, view page content</a></div>
<div id="navbarspace">


	<!-- Ohio State University Navigation Bar Example-Fixed Width: July 2008. See http://www.osu.edu/resources/ -->
	<title>The Ohio State University</title>
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
	<!-- 
	OSU Navigation Bar: Optional CSS
	Highly recommended but not required. 
	Resets style values for uniformity across all browsers. See file for more information.
	-->
	
	
	<!-- OSU Navigation Bar: Required Conditional CSS. -->
	<!--[if lte IE 6]>  
	<link rel="stylesheet" type="text/css" href="//dent.osu.edu/osu-navbar-media/css/navbar-ie6.css" />
	<![endif]-->
	<!--[if IE 7]>  
	<link rel="stylesheet" type="text/css" href="//dent.osu.edu/osu-navbar-media/css/navbar-ie7.css" />
	<![endif]-->
  
	<style type="text/css">
		<!--
		/*
		  	OSU Navigation Bar: Note for developers:
		    Page content following the OSU Navigation Bar must have a width assigned or 
			IE will not perform "skip to content".
		*/
		#page-content
		{
		  width: 100%;
		}
		#page-content h1, #page-content p
		{
		  padding: 1em 3em;
		}

		-->
	</style>

	<!-- OSU: Optional Favicon -->
	<link rel="icon" href="//dent.osu.edu//navbar/osu-navbar-media/img/favicon.ico" type="image/x-icon">
	<link rel="shortcut icon" href="//dent.osu.edu/navbar/osu-navbar-media/img/favicon.ico" type="image/x-icon"> 


	<!-- Begin OSU Navigation Bar -->
	<div id="osu-Navbar">
		<p>
			<a href="#page-content" id="skip" class="osu-semantic">skip to main content</a>
		</p>
		<h2 class="osu-semantic">OSU Navigation Bar</h2>
		<div id="osu-NavbarBreadcrumb">
			<p id="osu">
				<a title="The Ohio State University homepage" href="http://www.osu.edu/">The Ohio State University</a>
			</p>
			<p id="site-name">
			   College of Dentistry
			</p>
		</div>
		<div id="osu-NavbarLinks">
			<ul>
				<li><a href="http://www.osu.edu/help.php" title="OSU Help">Help</a></li>
				<li><a href="http://buckeyelink.osu.edu/" title="Buckeye Link">Buckeye Link</a></li>
				<li><a href="http://www.osu.edu/map/" title="Campus map">Map</a></li>
				<li><a href="http://www.osu.edu/findpeople.php" title="Find people at OSU">Find People</a></li>
				<li><a href="https://webmail.osu.edu" title="OSU Webmail">Webmail</a></li>
				<li id="searchbox">
					<form action="http://www.osu.edu/search/index.php" method="post">
						<fieldset>
							<legend><span class="osu-semantic">Search</span></legend>
							<label class="osu-semantic overlabel" for="search-field">Search Ohio State</label>
							<input type="text" alt-attribute="Search Ohio State" value="" name="searchOSU" class="textfield headerSearchInput" id="search-field">
							<button name="go" type="submit">Go</button>
						</fieldset>					
					</form>
			  </li>       
			</ul>
		</div>
	</div>
	<!-- Begin Page Content (not part of template) -->
	<!-- 
		OSU Navigation Bar: Note for developers:
		Page content following the OSU Navigation Bar must have a width assigned or IE will not perform "skip to content".
	-->


</div>

<!-- Page content begins here -->

<div id="content">
  <table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tbody><tr> 
      <td width="35%" valign="bottom"><a href="../patients/index.php" title="Return to Dentistry home"><img src="/dent.osu.edu/images/dentistry_logotype.gif" alt="The Ohio State University College of Dentistry" width="252" height="79" border="0"></a></td>
      <td width="470" align="center"> <div class="navleftrightrule"> 
          <!-- Change links to absolute paths-don't forget the go button graphic -->

<table width="470" border="0" cellspacing="0" cellpadding="0" id="topnav">
             <tbody><tr>
               <td width="33%" valign="top"><ol>
        <li value="1"><a href="http://dent.osu.edu/future_students/index.php">Future 
          Students</a></li>
        <li value="2"><a href="http://dent.osu.edu/patients/index.php">Patients</a></li>
        <li value="3"><a href="http://dent.osu.edu/alumnidev/">Alumni, Donors, &amp; Friends</a></li>
               </ol></td>
               <td width="34%" valign="top" class="leftrule"><ol>
        <li value="4"><a href="http://dent.osu.edu/current_students/index.php">Current Students</a></li>
        <li value="5"><a href="http://dent.osu.edu/faculty_staff/index.php">Faculty &amp; Staff</a></li>
               </ol></td>
               
    <td width="33%" valign="top" class="leftrule"> 
      <ol>
        <li value="6"><a href="http://dent.osu.edu">Dentistry Home</a></li>

               </ol>
			   </td>
             </tr>
      </tbody></table>
        </div></td>
      <td width="20%">&nbsp;</td>
    </tr>
    <tr bgcolor="#990000"> 
      <td width="35%" valign="top"><img src="/dent.osu.edu/images/y_descender.gif" alt=" " width="252" height="20"></td>
      <td width="470">&nbsp;</td>
      <td width="20%">&nbsp;</td>
    </tr>
        </tbody></table>
    <table width="100%" border="0" cellspacing="0" cellpadding="0"><tbody>
    <tr>
      <td valign="top"> 
        <!-- change links to absolute paths -->

<div id="leftnav">
	
  <ol>
    <li><a href="http://dent.osu.edu/College_Overview.php">College Overview</a></li>
	<li><a href="http://dent.osu.edu/academic_sections/index.php">Academic Sections</a></li>
	<li><a href="http://dent.osu.edu/Outreach">Community Outreach &amp; Engagement</a></li>
	<li><a href="http://dent.osu.edu/Research.php">Research</a></li>
	<li><a href="http://dent.osu.edu/sterilization/">Sterilization Monitoring Service</a></li>
	<li><a href="http://dent.osu.edu/Contact_Us.php">Contact Us</a></li>
	<li><a href="http://dent.osu.edu/Directions_and_Map.php">Directions &amp; Map</a>
<!--	    <div style="font-size: 8pt; color: red">&nbsp;SR 315 Construction<br>	-->
<!--	    &nbsp;Begins June 15, 2009</div></li>									-->
	</li><li><a href="http://dent.osu.edu/Directory.php">Directory</a></li>
  </ol>
</div>      </td>
<!--      <td valign="top"> <a name="sectionnav"></a> 
		<div id="level2nav" style="width: 165px; height: 272px"> 
          <h2>SMS</h2>
	      <ol>
            <li><a href="index.php">Home</a></li>
            <li><a href="about_service.php">About Our Service</a></li>
			<li><a href="self-study_CE.php">Continuing Education</a></li>
            <li><a href="price_list.php">Price List</a></li>
            <li><a href="profiles.php">Profiles</a></li>
            <li><a href="information_request.php">Information Request</a></li>
            <li><a href="contact_information.php">Contact Information</a></li>
          </ol>
        </div></td>-->
      <td colspan="3" valign="top"><a name="pagecontent"></a> 
        <div class="osudental-app" ng-app="osudental">
            <div class="container-fluid">
                <div ng-include src="'tpl/navbar.html'" ng-controller="NavBarCtrl"></div>
                <div id="ng-view" ui-view="primary"></div>
            </div>
        </div>
      </td>
    </tr>
  </tbody></table>
</div>

<!-- end of page content -->

<!-- footer -->
<!-- change graphic link to absolute path to your images folder -->

<div id="footer">
  <div id="copyright"> 
    <p>© 2013, The Ohio State University College of Dentistry<br>
    </p>
    <p>If you have trouble accessing this page and need to request an alternate 
      format, contact our <a href="mailto:dentwebmaster@osu.edu">webmaster</a>.</p>
</div>
  <a href="http://www.osu.edu"><img src="/dent.osu.edu/images/OSU-Wordmark-Gray-Horiz-RGBHEX.png" width="295" height="25" alt="The Ohio State University" border="0"></a>
  <p><a style="color: #b00; font-weight: bold;" href="http://dent.osu.edu">College of Dentistry</a><br>
    305 W. 12th Avenue<br>
    Columbus, OH 43210<br>
    Administration: (614) 292-2401<br>
    Clinics: (614) 292-2751<br>
  </p>
	
</div>
</html>