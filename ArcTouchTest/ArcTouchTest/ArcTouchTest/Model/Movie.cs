using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcTouchTest.Model
{
  public class Movie
  {
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("video")]
    public bool HasVideo { get; set; }
    [JsonProperty("vote_average")]
    public double VoteAverage { get; set; }
    [JsonProperty("title")]
    public string Title { get; set; }
    [JsonProperty("popularity")]
    public double Popularity { get; set; }
    [JsonProperty("poster_path")]
    public string PosterPath { get; set; }
    [JsonProperty("original_language")]
    public string Language { get; set; }
    [JsonProperty("original_title")]
    public string OriginalTitle { get; set; }
    [JsonProperty("genre_ids")]
    public IList<int> Genres { get; set; }
    [JsonProperty("backdrop_path")]
    public string BackdropPath { get; set; }
    [JsonProperty("adult")]
    public bool IsAdult { get; set; }
    [JsonProperty("overview")]
    public string Overview { get; set; }
    [JsonProperty("release_date")]
    public DateTime ReleaseDate { get; set; }
  }
}
