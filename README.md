LemmaGenerator
==============

LemmaGenerator creates lemmatizers for several European languages that you can customize.

This package is available on Nuget:
> Install-Package LemmaGenerator

This project was created by Matjaz Jursic and was retrieved on http://lemmatise.ijs.si/. He's the expert and did a great job so for all questions you should check his website.

Quickstart
----------------

If you just want to lemmatize words, you want to check the precompiled lemmatizer files here: https://github.com/AlexPoint/LemmaGenerator/tree/master/Test/Data.

Load the selected file in a stream a build a lemmatizer with it:

```csharp
var path = "/path/to/the/lemmatizer/file";
var stream = File.OpenRead(dataFilePath);
var lemmatizer = new Lemmatizer(stream);
var result = lemmatizer.Lemmatize("words");
Console.WriteLine(result);
// prints "word"
```

Customizing the lemmatizer
----------------

As mentioned above, you can customize your lemmatizer by using your own dictionary { word, lemma }.
For more information, check [Matjaz's website](http://lemmatise.ijs.si/).
