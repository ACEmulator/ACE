**Authentication**:<br>
The Web API uses [basic access authentication](https://en.wikipedia.org/wiki/Basic_access_authentication) and to access some of the data resources requires a valid account username and password for the underlying ACEmulator server.

If using CURL then the `-k` option is needed since certificates are self-signed.  For authenticated resources CURL needs `-u myusername:mypassword`.

If a resource is marked Anonymous then authorization isn't needed.  If you attempt to access a resource that isn't marked Anonymous without supplying valid credentials then you will get a `401 Unauthorized` response.

Authenticated requests assume the accesslevel of the account authenticated as a series of claims.

If a resource is marked `Requires: [Admin]`, that means unless your account includes the `Admin` claim then you won't be able to access the resource and you will get a `403 Forbidden` response.

If a resource is marked `RequiresAny: [Admin,Advocate,Developer,Envoy,Player,Sentinel]` then unless your authenticated account claims at least one of the required claims you won't be able to access the resource and you will get a `403 Forbidden` response.

**Web API**<br>
[documentation](https://documenter.getpostman.com/view/6422801/RznLHGgq)<br>
The Web API server listens at the base ACE port + 2, TCP, TLS, HTTPS, JSON, REST, depends on CryptoManager within ACE.Server project that Generates and saves self-signed certificates when it can't find them.  One for WebApi, and one for signing character transfers, in `C:\Users\<user>\AppData\Roaming\ACEmulator_<WorldName>\Certificates` on Windows or `/home/<user>/.config/ACEmulator_<WorldName>/Certificates` on Linux.  Place or replace webapi.pfx with a "valid" web server certificate if needed.  Self-Hosting using NancyFX/OWIN/Kestrel in its own project that depends on and wraps around ACE.Server.  WebSockets is a future possibility.  To run ACE with web API capability start the batch file: `\Source\ACE.Server.WebApi\bin\x64\Debug\netcoreapp2.1\start_server+webapi.bat`

**To enable and run ACEmulator with WebAPI:**<br>
1) Change the applicable setting to true in the configuration file, it should look like this after:

```
    "WebApi": {
        "Enabled": true
    },
```

2) Build the solution. 
3) Run the file `start_server+webapi.bat` in the output folder, which should be located somewhere like: `\Source\ACE.WebApiServer\bin\x64\Debug\netcoreapp2.1`

A postman collection is included in the source: `WebApi.postman_collection.json` - just import that into postman for a working test suite.

**Warning**<br>
If the world name in the server configuration is changed then the current transfers and certificate are lost unless the files are moved to the new location.  To mitigate this move or copy the contents of `C:\Users\<user>\AppData\Roaming\ACEmulator_<OldWorldName>\` to `C:\Users\<user>\AppData\Roaming\ACEmulator_<NewWorldName>\` replacing `<user>` with the applicable username, and `<OldWorldName>` and `<NewWorldName>` with applicable names.  For linux the path will usually be: `/home/<user>/.config/ACEmulator_<WorldName>/`.  If your world name contains special characters then the folder names will have them stripped out, allowed world name to folder name characters are: `abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789`.

**Migrations**:<br>

1. Via Web API, a request is made to migrate a character to another server.
2. Character snapshot is packaged and character is  placed in a "frozen" state.
3. Requester makes an API call to get the list of characters, of which is the migrating character in frozen state.
* At any time before the next step is initiated, or if the target server rejects the source server, an API request can be made to the source server to unfreeze the character and cancel the migration.
4. Requester makes a request to the target server (the character's new home).
5. Target server makes sure source server is trusted before downloading and rejects the request if it doesn't.
6. Target server downloads packaged snapshot from source server.
7. Source server permanently deletes character upon package download completion.
8. Target server performs cryptographic verification of the snapshot, and rejects it if it's been forged.
9. Target server adds the character to the requester's account.

Note:  Allowing migrations from itself enables cross account transfers and character renaming.

**Backups**:<br>
Via Web API, a request is made to backup a character.
Character snapshot is packaged and returned in the response to the requester.

**Imports**:<br>
**with imports enabled the exploitation flood gates are essentially open**<br>
requester makes a request to the target server API including in the request the snapshot package encoded in base64 and the new character name.
target server adds the character to the requester's account.

**Recommended settings for flawless contract groups:**<br>
allow backup: yes<br>
allow migrate: yes<br>
allow import: no<br>

**Limitations**:<br>
- ACE specific data constructs such as character options are not exported/imported.  This is to keep it as compatible as possible with LSD.

**Disclaimer**<br>
The migration feature requires the assumption that all server operators involved are non defectors.  A defector is someone in the trust group who does malicious or negligent action of any kind.  Providing a system that works for flawless trust groups is easy, but as soon as an attempt is made to eliminate the human element and commission the possibility of defectors the entire system breaks.  Manual approval process and/or central "antivirus" server may be something that someone might want to come up with in the future, but that's NOT what this is meant for.  **_As a server operator, DO NOT blame anyone but yourself upon defection(s) when using this system.  NOBODY BUT YOURSELF IS LIABLE_**