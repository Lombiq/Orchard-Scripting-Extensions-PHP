# Orchard Scripting Extensions: PHP Readme



## Project Description

A child module for Orchard Scripting Extensions for running PHP code inside Orchard.


## Documentation

This module depends on [Orchard Scripting Extensions](https://github.com/Lombiq/Orchard-Scripting-Extensions) and uses many of its features. Please install it first! (And also read that module's docs to see what you can do with it - and through it, with PHP).
PHP execution goes through the excellent [Phalanger](http://phalanger.codeplex.com/) library.

### Module overview

This module consists of two features:

- Orchard Scripting Extensions: PHP (OrchardHUN.Scripting.Php): the base, contains the core of PHP scripting support
- Orchard Scripting Extensions: PHP View Engine (OrchardHUN.Scripting.Php.ViewEngine): view engine for the support of .php views

### Base features

This feature gives you a PHP runtime (a service for executing PHP scripts).

PHP scripts automatically get a context through the `$_ORCHARD` superglobal variable. This contains the following keys:

- WORK_CONTEXT: this is the Orchard WorkContext
- ORCHARD_SERVICES: an IOrchardServices instance
- LAYOUT: the layout shape of the page

Samples:

	// Writing to the output. This is not always available, but the testbed writes it out.
	echo "Yes, this is PHP from Orchard.";
	
	// Regular PHP just works
	echo str_replace(array("don't", "ridiculous"), array("do", "awesome"), "I don't want to run PHP from Orchard, this is ridiculous.");
	
	function MyMistake()
	{
	    echo 'I play for you "My mistake"! ';
	    echo "My mistake, I have made my mistake! What a dreadful mistake! Is this mistake that I make!";
	}
	MyMistake();
	
	// Using Orchard services, here the WorkContext
	echo "This is the site's name: ".$_ORCHARD['WORK_CONTEXT']->CurrentSite->SiteName;
	
	// Or the Notifer
	$_ORCHARD['ORCHARD_SERVICES']->Notifier->Add(Orchard\UI\Notify\NotifyType::Warning, new Orchard\Localization\LocalizedString("This is a notification. Yes, this is still PHP."));
	
	// We could write wrappers for such complex calls, e.g.:
	function displayWarning($message)
	{
	    global $_ORCHARD;
	    $_ORCHARD['ORCHARD_SERVICES']->Notifier->Add(Orchard\UI\Notify\NotifyType::Warning, new Orchard\Localization\LocalizedString($message));
	}
	displayWarning("This is PHP!");
	
	// We can use .NET types as well
	echo System\DateTime::$Now->ToString();
	
	// Manipulating the layout
	// This adds the string "Hello!' to the markup of the layout's Body zone (this will just show up in the html source!).
	$_ORCHARD['LAYOUT']->Get("Body")->Add("Hello!"); 

Beware though that Clay and generally dynamic .NET objects don't work, so e.g. you can't modify the Layout shape directly (only by using the above technique).

### PHP View Engine

After enabling the feature you'll be able to use .php views that behave like ordinary PHP scripts and are fully interchangeable with their .cshtml counterparts.

Views get the full view context injected through the `$_ORCHARD['VIEW_CONTEXT']` superglobal variable and also the model separately as `$_ORCHARD['MODEL']`.

Samples, usable in .php views:

	// Branding.php
	<h1 id="branding"><a href="/"><?=$_ORCHARD['WORK_CONTEXT']->CurrentSite->SiteName?></a></h1>
	
	// Overriding body shape rendering with Parts.Common.Body.php
	<?=$_ORCHARD['MODEL']->Get("ContentPart")->Value->Text?>
	// or
	<?=$_ORCHARD['MODEL']->Get("Html")->ToString()?>


The module's source is available in two public source repositories, automatically mirrored in both directions with [Git-hg Mirror](https://githgmirror.com):

- [https://bitbucket.org/Lombiq/orchard-scripting-extensions-php](https://bitbucket.org/Lombiq/orchard-scripting-extensions-php) (Mercurial repository)
- [https://github.com/Lombiq/Orchard-Scripting-Extensions-PHP](https://github.com/Lombiq/Orchard-Scripting-Extensions-PHP) (Git repository)

Bug reports, feature requests and comments are warmly welcome, **please do so via GitHub**.
Feel free to send pull requests too, no matter which source repository you choose for this purpose.

This project is developed by [Lombiq Technologies Ltd](http://lombiq.com/). Commercial-grade support is available through Lombiq.