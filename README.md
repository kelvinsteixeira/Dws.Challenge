# Dws.Challenge - Artist search

This api is a facade for grabbing info about music artists from https://iws-brazil-labs-iws-recruiting-bands-staging.iwsbrazil.io/api/full.

### Swagger

```../swagger/index.html```

### Endpoints

```../artist``` : Returns the list of artists

```../artist/{id}``` : Returns the details of a given artist's id

### Next Steps

* Separate caching logic from ArtistService and Fetching. Pretty much a GetOrFetchService responsible by looking to the cache and fetching the data in case nothing is found from cache

* Use a Semaphore to avoid multi-thread issues when fetching and caching data

* Use a better caching implementation (redis for example) instead of an in memory solution

* Allow pagination

* Improve tests cases
