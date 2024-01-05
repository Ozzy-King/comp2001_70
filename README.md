# comp2001
this is my repository for the 70% on the 2001 course work. it consists of an API that can be tested through swagger, it connects to the backend database to perform CURD (create, update, read, delete) operations.

## CURD Operations (create, update, read, delete)

### profile
-------
#### GET Requests
1. /API/profile/getUserByEmail
2. /API/profile/getUserByName
##### descriptions
1. gets a public profile by email (must be exact).
2. gets a public profile by name (can be only part of name).

#### POST Requests
1. /API/profile/createUser
##### descriptions
1. supplied with the correct information in the Json body, it will create a account with that data.
##### request body schema
1. 
``` json
    {
        "email":"",
        "password":"" ,
        "firstname":"",
        "lastname":""
    }
``` 

#### DELETE Requests
1. /API/profile/deleteUser
2. /API/profile/deleteUserAdmin
##### descriptions
1. given the correct username and password that account will be deleted(archived).
2. given an email address along with an admins email and password the users account connected to the email will be deleted(archived).
##### request body schema
1. 
``` json
    {
        "email":"",
        "password":""
    }
```
2. 
``` json
    {
        "email":"",
        "adminemail":"",
        "adminpassword":""
    }
```

#### PUT Requests
1. /API/profile/updateUser
##### descriptions
1. given the correct email and password an account information passed along side with it will be inserted into that account.
##### request body schema
1. 
``` json
    {
        "email":"",
        "password":"" ,
        "firstname":"", //optional
        "lastname":"", //optional
        "newpassword":"", //optional
        "userphoto":"", //optional
        "aboutme":"", //optional
        "DOB":"", //optional
        "activityPreference":"", //optional
        "unit":"", //optional
        "height":"", //optional
        "weight":"", //optional
        "address":"" //optional
    }
``` 



### location
------
#### GET Requests
1. /API/location/getAllLocation
2. /API/location/getLocationByID
3. /API/location/getLocationByName
##### descriptions
1. gets all the locations currently stored on the database
2. gets a location based on the ID supplied
3. gets a location based on the name supplied

### tag
------
#### GET Requests
1. /API/tag/getAllLocation
2. /API/tag/getLocationByID
3. /API/tag/getLocationByName
##### descriptions
1. gets all the tags currently stored on the database
2. gets a tag based on the ID supplied
3. gets a tag based on the name supplied

### userTag
------
#### GET Requests
1. /API/userTag/getUserTagsByUser
2. /API/userTag/getUserTagsByTag
##### descriptions
1. returns all of the tags that are attached to the supplied user
2. gets all users that are attached to that specific tag

#### POST Requests
1. /API/userTag/createUserTag
##### descriptions
1. given email, password, and tag ID a new record is created
##### request body schema
1. 
``` json
    {
        "email":"",
        "password":"" ,
        "tagid":""
    }
``` 

#### POST Requests
1. /API/userTag/deleteUserTag
##### descriptions
1. given email, password, and tag ID the corresponding record is deleted
##### request body schema
1. 
``` json
    {
        "email":"",
        "password":"" ,
        "tagid":""
    }
``` 
