// <copyright file="ChatMember.cs" company="Bulat Tsydendorzhiev">
// Copyright (c) Bulat Tsydendorzhiev. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// </copyright>

namespace NetworkChat.ChatMember;

/// <summary>
/// Chat member that allows to write and read messages from sender.
/// </summary>
public class ChatMember
{
    /// <summary>
    /// The mechanism to exit from the chat.
    /// </summary>
    protected readonly CancellationTokenSource _cts;

    private string _messageRecipient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatMember"/> class.
    /// </summary>
    /// <param name="messageRecipient">Chat member who will get message from this member.</param>
    public ChatMember(string messageRecipient)
    {
        _messageRecipient = messageRecipient;
        _cts = new ();
    }

    /// <summary>
    /// Allows to write chat member message in the console.
    /// </summary>
    /// <param name="reader">Reader for getting the message.</param>
    /// <returns>Task that represent "ReadAsync" method.</returns>
    protected async Task ReadAsync(TextReader reader)
    {
        while (!_cts.IsCancellationRequested)
        {
            var message = await reader.ReadLineAsync(_cts.Token);
            if (message == "exit")
            {
                Stop();
                Console.WriteLine($"{_messageRecipient} decided to exit from the chat.");
                break;
            }

            Console.WriteLine($"{_messageRecipient}: {message}");
        }
    }

    /// <summary>
    /// Allows to write chat member message in the console.
    /// </summary>
    /// <param name="writer">Writer for sending the message for the recipient.</param>
    /// <returns>Task that represent "WriteAsync" method.</returns>
    protected async Task WriteAsync(TextWriter writer)
    {
        while (!_cts.IsCancellationRequested)
        {
            var message = Console.ReadLine();

            if (message == "exit")
            {
                Stop();
            }

            await writer.WriteLineAsync(message);
        }
    }

    /// <summary>
    /// Allows to stop the chat.
    /// </summary>
    protected void Stop()
        => _cts.Cancel();
}