# Contributing

1. Head over to Discussions for a list of what samples have been identified. Particularly, [this discussion](https://github.com/jsedlak/orleans-samples/discussions/2). If you want to add a new sample idea, leave a comment and note that you'll be working on it.
2. Fork the project and create a branch with your sample name
3. Create the sample solution
4. Write a `README.md` in the sample directory explaining the sample as well as how to use it
5. Add any HTTP calls to the bruno project stored in `/bruno`
6. Push your branch and create a PR back into `main`

## Sample Guidelines

- All samples should use Aspire to coordinate resources
- Samples are generally designed to focus on one topic at a time. Do your best to leave out options, infrastructure or patterns that are not required to effectively demonstrate the topic.
- Samples should use the Common projects (Common, ServiceDefaults, Streaming, et al) where possible to ensure a level of commonality
- Samples should not use external resources wherever necessary. Prefer using local emulators or containers to replace cloud hosted resources.
