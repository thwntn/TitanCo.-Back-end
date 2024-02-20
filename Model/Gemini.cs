namespace ReferenceModel;

public class MGemini
{
    public class Data(string input)
    {
        public List<Body> contents = [new(input)];

        public class Body(string input)
        {
            public List<object> parts = [new { text = input }];
        }
    }

    public class Response
    {
        public List<Content> candidates;

        public class Content
        {
            public Part content;
        }

        public class Part
        {
            public List<Text> parts;
        }

        public class Text
        {
            public string text;
        }
    }
}
