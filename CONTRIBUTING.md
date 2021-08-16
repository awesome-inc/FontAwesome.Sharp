# Contributing

We love contributions. To get started contributing you might need:

- [Get started with git](http://rogerdudler.github.io/git-guide)
- [How to create a pull request](https://help.github.com/articles/using-pull-requests)
- [An issue to work on](https://github.com/awesome-inc/FontAwesome.Sharp/labels/up-for-grabs) - We are
  on [Up for grabs](http://up-for-grabs.net/), our up for grabs issues are tagged `up-for-grabs`
- An understanding of how [we write tests](#writing-tests)

Once you know how to create a pull request and have an issue to work on, just post a comment saying you will work on it.
If you end up not being able to complete the task, please post another comment so others can pick it up.

Issues are also welcome, [failing tests](#writing-tests) are even more welcome.

## Contribution Guidelines

- Try to use feature branches rather than developing on master
- Please include tests covering the change
- The docs are now stored in the repository under the `Docs` folder, please include documentation updates with your PR

## Writing Tests

### 1. Find appropriate fixture

Find where your issue would logically sit, i.e. find the class closest to your issue.

### 2. Create a test method

We are currently using NUnit, so just create a descriptive test method and attribute it with `[Test]`.

### 3. Submit a pull request with the failing test

Even better include the fix, but a failing test is a great start.

## Updating FontAwesome

Since we are not automatically watching FontAwesome we invite you to add Pull Requests for updates on Font Awesome.
However, updating to newer version
of [@fortawesome/fontawesome-free](https://www.npmjs.com/package/@fortawesome/fontawesome-free)
and [@mdi/font](https://www.npmjs.com/package/@mdi/font) is automated via `Update-Fonts.ps1`, so

```console
$.\Update-Fonts.ps1
Updating node packages...
Updating web fonts...
Updating css...
Generating IconEnum classes...
```

Next, after successfuly running all tests

```console
build.bat /v:m /t:Test
```

Review and commit changes (e.g. `git diff/commit`).
