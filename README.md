# Codelouisville.DataLayer
This _**very**_ simple assembly provides a single Interface and a single abstract base class.

The base class will save and read data to and from a plain text file: you need to determine how that happens. 

See the **CodeLouisville.DataLayer.Sample** project for an example of how you can implement the BaseData class.

You could try saving as a comma or tab or pipe delimited file or even with [binary serialization](https://docs.microsoft.com/en-us/dotnet/standard/serialization/basic-serialization)


## Your implementation must
1. provide the path to the databse file to the base class constructor
1. provide an actual implementation of the following methods
    1. public abstract IList<T> Get();
        1. call GetFromDisk() to get the contents of your database file 
    1. public abstract void Save(IList<T> collection);
        1. call SaveToDisk(string serializedData) to save your database file to the disk
