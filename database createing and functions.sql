CREATE schema cw2;

-- location wont have values added t or removed it'll already be populated
CREATE TABLE CW2.location (
	ID int identity(0, 1)NOT NULL,
	name varchar(200) NOT NULL,
	CONSTRAINT PK_locationID PRIMARY KEY (ID)
);


--========================================profile and procedures======================================-----------------------
CREATE TABLE CW2.profile (
	email varchar(60) NOT NULL,
	joinDate date NOT NULL,
	userPhoto varchar(100) NOT NULL,
	
	firstName varchar(50) NOT NULL,
	lastName varchar(50) NOT NULL,
	userName varchar(100) NOT NULL,
	aboutMe varchar(500) NULL,
	DOB date NULL,
	
	password varchar(30) NOT NULL,

	activityTimePreference bit NULL,
	
	units bit NULL,
	height decimal(30,10) NULL,
	weight decimal(30,10) NULL,
	
	archived bit NOT NULL,
	address int NULL,

	CONSTRAINT PK_email PRIMARY KEY (email),
	CONSTRAINT FK_user_location FOREIGN KEY (address) REFERENCES CW2.location(ID),
);

--delete
CREATE PROCEDURE cw2.PRO_deleteUser @email varchar(60), @password varchar(30) as
	begin
		if @password = (select password from cw2.profile where email = @email)
			BEGIN 
				update cw2.profile
					set archived = 1
					where email = @email
				select 'success';
				return
			END
		select 'email or password isn''t correct';
		return
	end
	

--create user --return success if sccussful
CREATE PROCEDURE cw2.PRO_createUser @email varchar(60), @firstname varchar(50), @lastname varchar(50), @password varchar(30) AS
	begin
		--the the email already has an account linked and the passwords are the same then un archive account
		if (select count(email) from cw2.profile where email = @email) > 0 AND (select password from cw2.profile where email = @email) = @password
			begin
				if (select password from cw2.profile where email = @email) = @password
					BEGIN 
						update cw2.profile
						set archived = 0
						where email = @email
						select 'success';
						return;
					END
				ELSE 
					BEGIN 
						select 'account with email exists, enter correct password'
						return;
					END
			end
		
		--gets a random picture
		DECLARE @PicVar AS VARCHAR(30);
		DECLARE @RANDOMNUM INT = cast(RAND()*100000 as int) %5;
			IF @RANDOMNUM = 0
					SET @PicVar = 'defaultPhoto/generic1.png'
			ELSE IF @RANDOMNUM = 1
					SET @PicVar = 'defaultPhoto/generic2.png'
			ELSE IF @RANDOMNUM = 2
					SET @PicVar = 'defaultPhoto/generic3.png'
			ELSE
					SET @PicVar = 'defaultPhoto/generic4.png'

		insert into cw2.profile(email, joinDate, userPhoto, firstName, lastName, userName, password, activityTimePreference, units, archived)
			values(@email, GETDATE(), @PicVar, @firstname, @lastname, CONCAT(@firstname, ' ', @lastname), @password, 0, 0, 0)
		select 'success';
	end

--update user
create procedure cw2.PRO_updateUser @email varchar(60), @userPhoto varchar(100), @firstname varchar(50), @lastname varchar(50), @aboutme varchar(500), @DOB date, @password varchar(30), @activityPreference bit, @units bit, @height decimal(30,10), @weight decimal(30,10), @address int as
	begin
		--error check
		if @email = NULL
		begin
			select 'Must Give Email'
			return
		end
		
		
		--set user photo
		if @userPhoto != NULL
		begin
			update cw2.profile 
				set userPhoto = @userPhoto
				where email = @email
		end	
		--set users firstname and username
		if @firstname != NULL
		begin
			update cw2.profile 
				set firstName = @firstname, userName = CONCAT(@firstname, ' ', (select lastName from cw2.profile where email = @email))
				where email = @email
		end
		--set users lastname and username
		if @lastname != NULL
		begin
			update cw2.profile 
				set lastName = @lastname, userName = CONCAT((select firstName from cw2.profile where email = @email), ' ', @lastname)
				where email = @email		
		END
		--set about me
		if @aboutme != NULL
		begin
			update cw2.profile 
				set aboutMe = @aboutme
				where email = @email		
		END		
		-- set date of birth
		if @DOB != NULL
		begin
			update cw2.profile 
				set DOB = @DOB
				where email = @email		
		END		
		-- set password
		if @password != NULL
		begin
			update cw2.profile 
				set password = @password
				where email = @email		
		END	
		-- set activity preference
		if @activityPreference != NULL
		begin
			update cw2.profile 
				set activityTimePreference = @activityPreference
				where email = @email		
		END	
		-- set uint type 0 metric 1 imperial
		if @units != NULL
		begin
			update cw2.profile 
				set units = @units
				where email = @email		
		END	
		-- set height
		if @height != NULL
		begin
			update cw2.profile 
				set height = @height
				where email = @email		
		END			
		--set weight
		if @weight != NULL
		begin
			update cw2.profile 
				set weight = @weight
				where email = @email		
		END	
		-- set address
		if @address != NULL
		begin
			update cw2.profile 
				set address = @address
				where email = @email		
		END	
		select 'success';
		return
	end
	
	
	
	
--=====================tags wont have values added t or removed it'll already be populated======================---------------
CREATE TABLE CW2.activityTag (
	ID int identity(0, 1) NOT NULL,
	name varchar(20) NOT NULL,
	
	CONSTRAINT PK_activityTagName PRIMARY KEY (ID)
);


--=================composite key on tag name and user email () this is a juction table=============================-----------------
CREATE TABLE CW2.userActivityTag (
	tagID int NOT NULL,
	useremail varchar(60) NOT NULL,
	
	CONSTRAINT PK_userActivityTagID PRIMARY KEY (tagID, useremail),
	CONSTRAINT FK_userActivityTag_activityTag FOREIGN KEY (tagID) REFERENCES CW2.activityTag(ID),
	CONSTRAINT FK_userActivityTag_user FOREIGN KEY (userEmail) REFERENCES CW2.profile(email)
);

--create (tag - user)
create procedure cw2.UAT_createUserTag @email varchar(60), @password varchar(30), @tagID int as 
	begin
		if (select password from cw2.profile where email = @email) = @password
			begin
				INSERT into cw2.userActivityTag (tagID, useremail)
					values(@email, @tagID)
				select 'success';
				return
			end
		select 'email or password isn''t correct';
		return
	end

--delete
create procedure cw2.UAT_deleteUserTag @email varchar(60), @password varchar(30), @tagID int as 
	begin
		if (select password from cw2.profile where email = @email) = @password
			begin
				delete from cw2.userActivityTag
					where useremail = @email
				select 'success';
				return
			end
		select 'email or password isn''t correct';
		return
	end

--===========================views======================================--------------------
-- public faceing data (join tables when needed) --userName,aboutMe, location, join date, userPhoto
CREATE view CW2.publicUserData as
	select pro.joinDate, pro.userPhoto, pro.userName, pro.aboutMe, loc.name as location
	from CW2.profile pro
	left join cw2.location loc
		on pro.address = loc.ID;
		




