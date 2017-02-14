# Simple UserService implementation
[UserService](https://github.com/ShiloDenis1997/EPAM.RD.2017F.Shilo.01/blob/master/UserServiceLibrary/UserService.cs) implements 
[IUserService](https://github.com/ShiloDenis1997/EPAM.RD.2017F.Shilo.01/blob/master/UserServiceLibrary/Interfaces/IUserService.cs) which provides following methods:

* Add new user
* Remove user
* Search user by predicate

Service uses [LinkedList](https://msdn.microsoft.com/ru-ru/library/he2s3bh7%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396)<[User](https://github.com/ShiloDenis1997/EPAM.RD.2017F.Shilo.01/blob/master/UserServiceLibrary/User.cs)> as a local storage.

Also service provides it's own [exceptions hierarchy](https://github.com/ShiloDenis1997/EPAM.RD.2017F.Shilo.01/tree/master/UserServiceLibrary/Exceptions).
